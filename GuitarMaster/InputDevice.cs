// Copyright (c) 2009, Tom Lokovic
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice,
//       this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Midi
{
    /// <summary>
    /// A MIDI input device.
    /// </summary>
    /// <remarks>
    /// <para>Each instance of this class describes a MIDI input device installed on the system.
    /// You cannot create your own instances, but instead must go through the
    /// <see cref="InstalledDevices"/> property to find which devices are available.  You may wish
    /// to examine the <see cref="DeviceBase.Name"/> property of each one and present the user with
    /// a choice of which device(s) to use.</para>
    /// <para>Open an input device with <see cref="Open"/> and close it with <see cref="Close"/>.
    /// While it is open, you may arrange to start receiving messages with
    /// <see cref="StartReceiving"/> and then stop receiving them with <see cref="StopReceiving"/>.
    /// An input device can only receive messages when it is both open and started.</para>
    /// <para>Incoming messages are routed to the corresponding events, such as <see cref="NoteOn"/>
    /// and <see cref="ProgramChange"/>.  The event handlers are invoked on a background thread
    /// which is started in <see cref="StartReceiving"/> and stopped in <see cref="StopReceiving"/>.
    /// </para>
    /// <para>As each message is received, it is assigned a timestamp in one of two ways.  If
    /// <see cref="StartReceiving"/> is called with a <see cref="Clock"/>, then each message is
    /// assigned a time by querying the clock's <see cref="Clock.BeatTime"/> property.  If
    /// <see cref="StartReceiving"/> is called with null, then each message is assigned a time
    /// based on the number of seconds since <see cref="StartReceiving"/> was called.</para>
    /// </remarks>
    /// <threadsafety static="true" instance="true"/>
    /// <seealso cref="Clock"/>
    /// <seealso cref="InputDevice"/>
    public class InputDevice : DeviceBase
    {
        #region Delegates

        /// <summary>
        /// Delegate called when an input device receives a Note On message.
        /// </summary>
        public delegate void NoteOnHandler(NoteOnMessage msg);

        /// <summary>
        /// Delegate called when an input device receives a Note Off message.
        /// </summary>
        public delegate void NoteOffHandler(NoteOffMessage msg);

        /// <summary>
        /// Delegate called when an input device receives a Control Change message.
        /// </summary>
        public delegate void ControlChangeHandler(ControlChangeMessage msg);

        /// <summary>
        /// Delegate called when an input device receives a Program Change message.
        /// </summary>
        public delegate void ProgramChangeHandler(ProgramChangeMessage msg);

        /// <summary>
        /// Delegate called when an input device receives a Pitch Bend message.
        /// </summary>
        public delegate void PitchBendHandler(PitchBendMessage msg);

        #endregion

        #region Events

        /// <summary>
        /// Event called when an input device receives a Note On message.
        /// </summary>
        public event NoteOnHandler NoteOn;

        /// <summary>
        /// Event called when an input device receives a Note Off message.
        /// </summary>
        public event NoteOffHandler NoteOff;

        /// <summary>
        /// Event called when an input device receives a Control Change message.
        /// </summary>
        public event ControlChangeHandler ControlChange;

        /// <summary>
        /// Event called when an input device receives a Program Change message.
        /// </summary>
        public event ProgramChangeHandler ProgramChange;

        /// <summary>
        /// Event called when an input device receives a Pitch Bend message.
        /// </summary>
        public event PitchBendHandler PitchBend;

        #endregion

        #region Public Methods and Properties

        /// <summary>
        /// List of input devices installed on this system.
        /// </summary>
        public static ReadOnlyCollection<InputDevice> InstalledDevices
        {
            get
            {
                lock (staticLock)
                {
                    if (installedDevices == null)
                    {
                        installedDevices = MakeDeviceList();
                    }
                    return new ReadOnlyCollection<InputDevice>(installedDevices);
                }
            }
        }

        /// <summary>
        /// True if this device has been successfully opened.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                if (isInsideInputHandler)
                {
                    return true;
                }
                lock (this)
                {
                    return isOpen;
                }
            }
        }

        /// <summary>
        /// Opens this input device.
        /// </summary>
        /// <exception cref="InvalidOperationException">The device is already open.</exception>
        /// <exception cref="DeviceException">The device cannot be opened.</exception>
        /// <remarks>Note that Open() establishes a connection to the device, but no messages will
        /// be received until <see cref="StartReceiving"/> is called.</remarks>
        public void Open()
        {
            if (isInsideInputHandler)
            {
                throw new InvalidOperationException("Device is open.");
            }
            lock (this)
            {
                CheckNotOpen();
                CheckReturnCode(Win32API.midiInOpen(out handle, deviceId,
                    new Win32API.MidiInProc(InputCallback), (UIntPtr)0));
                isOpen = true;
            }
        }

        /// <summary>
        /// Closes this input device.
        /// </summary>
        /// <exception cref="InvalidOperationException">The device is not open or is still
        /// receiving.</exception>
        /// <exception cref="DeviceException">The device cannot be closed.</exception>
        public void Close()
        {
            if (isInsideInputHandler)
            {
                throw new InvalidOperationException("Device is receiving.");
            }
            lock (this)
            {
                CheckOpen();
                CheckReturnCode(Win32API.midiInClose(handle));
                isOpen = false;
            }
        }

        /// <summary>
        /// True if this device is receiving messages.
        /// </summary>
        public bool IsReceiving
        {
            get
            {
                if (isInsideInputHandler)
                {
                    return true;
                }
                lock (this)
                {
                    return isReceiving;
                }
            }
        }

        /// <summary>
        /// Starts this input device receiving messages.
        /// </summary>
        /// <param name="clock">If non-null, the clock's <see cref="Clock.BeatTime"/> property will
        /// be used to assign a timestamp to each incoming message.  If null, timestamps will be in
        /// seconds since StartReceiving() was called.</param>
        /// <exception cref="InvalidOperationException">The device is not open or is already
        /// receiving.
        /// </exception>
        /// <exception cref="DeviceException">The device cannot start receiving.</exception>
        /// <remarks>
        /// <para>This method launches a background thread to listen for input events, and as events
        /// are received, the event handlers are invoked on that background thread.  Event handlers
        /// should be written to work from a background thread.  (For example, if they want to
        /// update the GUI, they may need to BeginInvoke to arrange for GUI updates to happen on
        /// the correct thread.)</para>
        /// <para>The background thread which is created by this method is joined (shut down) in
        /// <see cref="StopReceiving"/>.</para>
        /// </remarks>
        public void StartReceiving(Clock clock)
        {
            if (isInsideInputHandler)
            {
                throw new InvalidOperationException("Device is receiving.");
            }
            lock (this)
            {
                CheckOpen();
                CheckNotReceiving();
                CheckReturnCode(Win32API.midiInStart(handle));
                isReceiving = true;
                this.clock = clock;
            }
        }
        
        /// <summary>
        /// Stops this input device from receiving messages.
        /// </summary>
        /// <remarks>
        /// <para>This method waits for all in-progress input event handlers to finish, and then
        /// joins (shuts down) the background thread that was created in
        /// <see cref="StartReceiving"/>.  Thus, when this function returns you can be sure that no
        /// more event handlers will be invoked.</para>
        /// <para>It is illegal to call this method from an input event handler (ie, from the
        /// background thread), and doing so throws an exception. If an event handler really needs
        /// to call this method, consider using BeginInvoke to schedule it on another thread.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">The device is not open; is not receiving;
        /// or called from within an event handler (ie, from the background thread).</exception>
        /// <exception cref="DeviceException">The device cannot start receiving.</exception>
        public void StopReceiving()
        {
            if (isInsideInputHandler)
            {
                throw new InvalidOperationException(
                    "Can't call StopReceiving() from inside an input handler.");
            }
            lock (this)
            {
                CheckReceiving();
                CheckReturnCode(Win32API.midiInStop(handle));
                clock = null;
                isReceiving = false;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Makes sure rc is MidiWin32Wrapper.MMSYSERR_NOERROR.  If not, throws an exception with an
        /// appropriate error message.
        /// </summary>
        /// <param name="rc"></param>
        private static void CheckReturnCode(Win32API.MMRESULT rc)
        {
            if (rc != Win32API.MMRESULT.MMSYSERR_NOERROR)
            {
                StringBuilder errorMsg = new StringBuilder(128);
                rc = Win32API.midiInGetErrorText(rc, errorMsg);
                if (rc != Win32API.MMRESULT.MMSYSERR_NOERROR)
                {
                    throw new DeviceException("no error details");
                }
                throw new DeviceException(errorMsg.ToString());
            }
        }

        /// <summary>
        /// Throws a MidiDeviceException if this device is not open.
        /// </summary>
        private void CheckOpen()
        {
            if (!isOpen)
            {
                throw new InvalidOperationException("Device is not open.");
            }
        }

        /// <summary>
        /// Throws a MidiDeviceException if this device is open.
        /// </summary>
        private void CheckNotOpen()
        {
            if (isOpen)
            {
                throw new InvalidOperationException("Device is open.");
            }
        }

        /// <summary>
        /// Throws a MidiDeviceException if this device is not receiving.
        /// </summary>
        private void CheckReceiving()
        {
            if (!isReceiving)
            {
                throw new DeviceException("device not receiving");
            }
        }

        /// <summary>
        /// Throws a MidiDeviceException if this device is receiving.
        /// </summary>
        private void CheckNotReceiving()
        {
            if (isReceiving)
            {
                throw new DeviceException("device receiving");
            }
        }

        /// <summary>
        /// Private Constructor, only called by the getter for the InstalledDevices property.
        /// </summary>
        /// <param name="deviceId">Position of this device in the list of all devices.</param>
        /// <param name="caps">Win32 Struct with device metadata</param>
        private InputDevice(UIntPtr deviceId, Win32API.MIDIINCAPS caps)
            : base(caps.szPname)
        {
            this.deviceId = deviceId;
            this.caps = caps;
            this.isOpen = false;
            this.clock = null;
        }

        /// <summary>
        /// Private method for constructing the array of MidiInputDevices by calling the Win32 api.
        /// </summary>
        /// <returns></returns>
        private static InputDevice[] MakeDeviceList()
        {
            uint inDevs = Win32API.midiInGetNumDevs();
            InputDevice[] result = new InputDevice[inDevs];
            for (uint deviceId = 0; deviceId < inDevs; deviceId++)
            {
                Win32API.MIDIINCAPS caps = new Win32API.MIDIINCAPS();
                Win32API.midiInGetDevCaps((UIntPtr)deviceId, out caps);
                result[deviceId] = new InputDevice((UIntPtr)deviceId, caps);
            }
            return result;
        }

        /// <summary>
        /// The input callback for midiOutOpen.
        /// </summary>
        private void InputCallback(Win32API.HMIDIIN hMidiIn, Win32API.MidiInMessage wMsg,
            UIntPtr dwInstance, UIntPtr dwParam1, UIntPtr dwParam2)
        {
            isInsideInputHandler = true;
            try
            {
                if (wMsg == Win32API.MidiInMessage.MIM_DATA)
                {
                    Channel channel;
                    Note note;
                    int velocity;
                    Control control;
                    int value;
                    Instrument instrument;
                    UInt32 win32Timestamp;
                    if (ShortMsg.IsNoteOn(dwParam1, dwParam2))
                    {
                        if (NoteOn != null)
                        {
                            ShortMsg.DecodeNoteOn(dwParam1, dwParam2, out channel, out note,
                                out velocity, out win32Timestamp);
                            NoteOn(new NoteOnMessage(this, channel, note, velocity,
                                clock == null ? win32Timestamp/1000f : clock.BeatTime));
                        }
                    }
                    else if (ShortMsg.IsNoteOff(dwParam1, dwParam2))
                    {
                        if (NoteOff != null)
                        {
                            ShortMsg.DecodeNoteOff(dwParam1, dwParam2, out channel, out note,
                                out velocity, out win32Timestamp);
                            NoteOff(new NoteOffMessage(this, channel, note, velocity,
                                clock == null ? win32Timestamp / 1000f : clock.BeatTime));
                        }
                    }
                    else if (ShortMsg.IsControlChange(dwParam1, dwParam2))
                    {
                        if (ControlChange != null)
                        {
                            ShortMsg.DecodeControlChange(dwParam1, dwParam2, out channel,
                                out control, out value, out win32Timestamp);
                            ControlChange(new ControlChangeMessage(this, channel, control, value,
                                clock == null ? win32Timestamp / 1000f : clock.BeatTime));
                        }
                    }
                    else if (ShortMsg.IsProgramChange(dwParam1, dwParam2))
                    {
                        if (ProgramChange != null)
                        {
                            ShortMsg.DecodeProgramChange(dwParam1, dwParam2, out channel,
                                out instrument, out win32Timestamp);
                            ProgramChange(new ProgramChangeMessage(this, channel, instrument,
                                clock == null ? win32Timestamp / 1000f : clock.BeatTime));
                        }
                    }
                    else if (ShortMsg.IsPitchBend(dwParam1, dwParam2))
                    {
                        if (PitchBend != null)
                        {
                            ShortMsg.DecodePitchBend(dwParam1, dwParam2, out channel,
                                out value, out win32Timestamp);
                            PitchBend(new PitchBendMessage(this, channel, value,
                                clock == null ? win32Timestamp / 1000f : clock.BeatTime));
                        }
                    }
                    else
                    {
                        // Unsupported messages are ignored.
                    }
                }
            }
            finally
            {
                isInsideInputHandler = false;
            }
        }

        #endregion

        #region Private Fields

        // Access to the global state is guarded by lock(staticLock).
        private static Object staticLock = new Object();
        private static InputDevice[] installedDevices = null;

        // The fields initialized in the constructor never change after construction,
        // so they don't need to be guarded by a lock.
        private UIntPtr deviceId;
        private Win32API.MIDIINCAPS caps;

        // Access to the Open/Close state is guarded by lock(this).
        private bool isOpen;
        private bool isReceiving;
        private Clock clock;
        private Win32API.HMIDIIN handle;

        /// <summary>
        /// Thread-local, set to true when called by an input handler, false in all other threads.
        /// </summary>
        [ThreadStatic]
        static bool isInsideInputHandler = false;

        #endregion
    }
}