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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midi
{
    /// <summary>
    /// MIDI Notes.
    /// </summary>
    /// <remarks>
    /// <para>MIDI defines 127 distinct notes, in semitone intervals, ranging from C five octaves
    /// below middle C, up to G five octaves above middle C.  This covers several octaves above and
    /// below the range of a normal 88-key piano.</para>
    /// <para>These 127 notes are the only ones directly expressible in MIDI. Precise
    /// variations in frequency can be achieved with <see cref="OutputDevice.SendPitchBend">Pitch
    /// Bend</see> messages, though Pitch Bend messages apply to the whole channel at once.</para>
    /// <para>In this enum, each note name includes the letter name, followed by Sharp if
    /// necessary, followed by the octave number, where octaves are numbered with standard piano
    /// terminology: Middle C is in octave four.  Thus octaves range from -1 to 9.  (This is
    /// different from "MIDI octaves", which range from -5 to 5.)</para>
    /// <para>This enum has extension methods, such as <see cref="NoteExtensionMethods.Name"/> and
    /// <see cref="NoteExtensionMethods.IsValid"/>, defined in <see cref="NoteExtensionMethods"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="Note"/>
    /// <seealso cref="Interval"/>
    /// 
    
    public enum Note
    {
        /// <summary>C in octave -1.</summary>
        CNeg1 = 0,
        /// <summary>C# in octave -1.</summary>
        CSharpNeg1 = 1,
        /// <summary>D in octave -1.</summary>
        DNeg1 = 2,
        /// <summary>D# in octave -1.</summary>
        DSharpNeg1 = 3,
        /// <summary>E in octave -1.</summary>
        ENeg1 = 4,
        /// <summary>F in octave -1.</summary>
        FNeg1 = 5,
        /// <summary>F# in octave -1.</summary>
        FSharpNeg1 = 6,
        /// <summary>G in octave -1.</summary>
        GNeg1 = 7,
        /// <summary>G# in octave -1.</summary>
        GSharpNeg1 = 8,
        /// <summary>A in octave -1.</summary>
        ANeg1 = 9,
        /// <summary>A# in octave -1.</summary>
        ASharpNeg1 = 10,
        /// <summary>B in octave -1.</summary>
        BNeg1 = 11,

        /// <summary>C in octave 0.</summary>
        C0 = 12,
        /// <summary>C# in octave 0.</summary>
        CSharp0 = 13,
        /// <summary>D in octave 0.</summary>
        D0 = 14,
        /// <summary>D# in octave 0.</summary>
        DSharp0 = 15,
        /// <summary>E in octave 0.</summary>
        E0 = 16,
        /// <summary>F in octave 0.</summary>
        F0 = 17,
        /// <summary>F# in octave 0.</summary>
        FSharp0 = 18,
        /// <summary>G in octave 0.</summary>
        G0 = 19,
        /// <summary>G# in octave 0.</summary>
        GSharp0 = 20,
        /// <summary>A in octave 0.</summary>
        A0 = 21,
        /// <summary>A# in octave 0, usually the lowest key on an 88-key keyboard.</summary>
        ASharp0 = 22,
        /// <summary>B in octave 0.</summary>
        B0 = 23,

        /// <summary>C in octave 1.</summary>
        C1 = 24,
        /// <summary>C# in octave 1.</summary>
        CSharp1 = 25,
        /// <summary>D in octave 1.</summary>
        D1 = 26,
        /// <summary>D# in octave 1.</summary>
        DSharp1 = 27,
        /// <summary>E in octave 1.</summary>
        E1 = 28,
        /// <summary>F in octave 1.</summary>
        F1 = 29,
        /// <summary>F# in octave 1.</summary>
        FSharp1 = 30,
        /// <summary>G in octave 1.</summary>
        G1 = 31,
        /// <summary>G# in octave 1.</summary>
        GSharp1 = 32,
        /// <summary>A in octave 1.</summary>
        A1 = 33,
        /// <summary>A# in octave 1.</summary>
        ASharp1 = 34,
        /// <summary>B in octave 1.</summary>
        B1 = 35,

        /// <summary>C in octave 2.</summary>
        C2 = 36,
        /// <summary>C# in octave 2.</summary>
        CSharp2 = 37,
        /// <summary>D in octave 2.</summary>
        D2 = 38,
        /// <summary>D# in octave 2.</summary>
        DSharp2 = 39,
        /// <summary>E in octave 2.</summary>
        E2 = 40,
        /// <summary>F in octave 2.</summary>
        F2 = 41,
        /// <summary>F# in octave 2.</summary>
        FSharp2 = 42,
        /// <summary>G in octave 2.</summary>
        G2 = 43,
        /// <summary>G# in octave 2.</summary>
        GSharp2 = 44,
        /// <summary>A in octave 2.</summary>
        A2 = 45,
        /// <summary>A# in octave 2.</summary>
        ASharp2 = 46,
        /// <summary>B in octave 2.</summary>
        B2 = 47,

        /// <summary>C in octave 3.</summary>
        C3 = 48,
        /// <summary>C# in octave 3.</summary>
        CSharp3 = 49,
        /// <summary>D in octave 3.</summary>
        D3 = 50,
        /// <summary>D# in octave 3.</summary>
        DSharp3 = 51,
        /// <summary>E in octave 3.</summary>
        E3 = 52,
        /// <summary>F in octave 3.</summary>
        F3 = 53,
        /// <summary>F# in octave 3.</summary>
        FSharp3 = 54,
        /// <summary>G in octave 3.</summary>
        G3 = 55,
        /// <summary>G# in octave 3.</summary>
        GSharp3 = 56,
        /// <summary>A in octave 3.</summary>
        A3 = 57,
        /// <summary>A# in octave 3.</summary>
        ASharp3 = 58,
        /// <summary>B in octave 3.</summary>
        B3 = 59,

        /// <summary>C in octave 4, also known as Middle C.</summary>
        C4 = 60,
        /// <summary>C# in octave 4.</summary>
        CSharp4 = 61,
        /// <summary>D in octave 4.</summary>
        D4 = 62,
        /// <summary>D# in octave 4.</summary>
        DSharp4 = 63,
        /// <summary>E in octave 4.</summary>
        E4 = 64,
        /// <summary>F in octave 4.</summary>
        F4 = 65,
        /// <summary>F# in octave 4.</summary>
        FSharp4 = 66,
        /// <summary>G in octave 4.</summary>
        G4 = 67,
        /// <summary>G# in octave 4.</summary>
        GSharp4 = 68,
        /// <summary>A in octave 4.</summary>
        A4 = 69,
        /// <summary>A# in octave 4.</summary>
        ASharp4 = 70,
        /// <summary>B in octave 4.</summary>
        B4 = 71,

        /// <summary>C in octave 5.</summary>
        C5 = 72,
        /// <summary>C# in octave 5.</summary>
        CSharp5 = 73,
        /// <summary>D in octave 5.</summary>
        D5 = 74,
        /// <summary>D# in octave 5.</summary>
        DSharp5 = 75,
        /// <summary>E in octave 5.</summary>
        E5 = 76,
        /// <summary>F in octave 5.</summary>
        F5 = 77,
        /// <summary>F# in octave 5.</summary>
        FSharp5 = 78,
        /// <summary>G in octave 5.</summary>
        G5 = 79,
        /// <summary>G# in octave 5.</summary>
        GSharp5 = 80,
        /// <summary>A in octave 5.</summary>
        A5 = 81,
        /// <summary>A# in octave 5.</summary>
        ASharp5 = 82,
        /// <summary>B in octave 5.</summary>
        B5 = 83,

        /// <summary>C in octave 6.</summary>
        C6 = 84,
        /// <summary>C# in octave 6.</summary>
        CSharp6 = 85,
        /// <summary>D in octave 6.</summary>
        D6 = 86,
        /// <summary>D# in octave 6.</summary>
        DSharp6 = 87,
        /// <summary>E in octave 6.</summary>
        E6 = 88,
        /// <summary>F in octave 6.</summary>
        F6 = 89,
        /// <summary>F# in octave 6.</summary>
        FSharp6 = 90,
        /// <summary>G in octave 6.</summary>
        G6 = 91,
        /// <summary>G# in octave 6.</summary>
        GSharp6 = 92,
        /// <summary>A in octave 6.</summary>
        A6 = 93,
        /// <summary>A# in octave 6.</summary>
        ASharp6 = 94,
        /// <summary>B in octave 6.</summary>
        B6 = 95,

        /// <summary>C in octave 7.</summary>
        C7 = 96,
        /// <summary>C# in octave 7.</summary>
        CSharp7 = 97,
        /// <summary>D in octave 7.</summary>
        D7 = 98,
        /// <summary>D# in octave 7.</summary>
        DSharp7 = 99,
        /// <summary>E in octave 7.</summary>
        E7 = 100,
        /// <summary>F in octave 7.</summary>
        F7 = 101,
        /// <summary>F# in octave 7.</summary>
        FSharp7 = 102,
        /// <summary>G in octave 7.</summary>
        G7 = 103,
        /// <summary>G# in octave 7.</summary>
        GSharp7 = 104,
        /// <summary>A in octave 7.</summary>
        A7 = 105,
        /// <summary>A# in octave 7.</summary>
        ASharp7 = 106,
        /// <summary>B in octave 7.</summary>
        B7 = 107,

        /// <summary>C in octave 8, usually the highest key on an 88-key keyboard.</summary>
        C8 = 108,
        /// <summary>C# in octave 8.</summary>
        CSharp8 = 109,
        /// <summary>D in octave 8.</summary>
        D8 = 110,
        /// <summary>D# in octave 8.</summary>
        DSharp8 = 111,
        /// <summary>E in octave 8.</summary>
        E8 = 112,
        /// <summary>F in octave 8.</summary>
        F8 = 113,
        /// <summary>F# in octave 8.</summary>
        FSharp8 = 114,
        /// <summary>G in octave 8.</summary>
        G8 = 115,
        /// <summary>G# in octave 8.</summary>
        GSharp8 = 116,
        /// <summary>A in octave 8.</summary>
        A8 = 117,
        /// <summary>A# in octave 8.</summary>
        ASharp8 = 118,
        /// <summary>B in octave 8.</summary>
        B8 = 119,

        /// <summary>C in octave 9.</summary>
        C9 = 120,
        /// <summary>C# in octave 9.</summary>
        CSharp9 = 121,
        /// <summary>D in octave 9.</summary>
        D9 = 122,
        /// <summary>D# in octave 9.</summary>
        DSharp9 = 123,
        /// <summary>E in octave 9.</summary>
        E9 = 124,
        /// <summary>F in octave 9.</summary>
        F9 = 125,
        /// <summary>F# in octave 9.</summary>
        FSharp9 = 126,
        /// <summary>G in octave 9.</summary>
        G9 = 127
    }
    /// <summary>
    /// Extension methods for the Note enum.
    /// </summary>
    public static class MyNote
    {
        public static Note Note(int note, int octava)
        {
            switch (note)
            {
                case 1:
                    return Midi.Note.A4;
                case 2:
                    return Midi.Note.B4;
                case 3:
                    return Midi.Note.C4;
                case 4:
                    return Midi.Note.D4;
                case 5:
                    return Midi.Note.E4;
                case 6:
                    return Midi.Note.F4;
                case 7:
                    return Midi.Note.G4;
            }
            return Midi.Note.A4;
        }
    }
    public static class NoteExtensionMethods
    {        
        /// <summary>
        /// Returns true if note is valid.
        /// </summary>
        /// <param name="note">The note to test.</param>
        /// <returns>True if the note is valid.</returns>
        public static bool IsValid(this Note note)
        {
            return (int)note >= 0 && (int)note < 128;
        }

        /// <summary>
        /// Throws an exception if note is invalid.
        /// </summary>
        /// <param name="note">The note to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException">The note is out-of-range.</exception>
        public static void Validate(this Note note)
        {
            if (!note.IsValid())
            {
                throw new ArgumentOutOfRangeException("Note out of range");
            }
        }

        /// <summary>
        /// Returns the octave number of the given note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns>The octave.  For example, "C4"'s octave is 4.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The note is out-of-range.</exception>
        public static int Octave(this Note note)
        {
            note.Validate();
            return (int)note / 12 - 1;
        }

        /// <summary>
        /// Returns the name of the specified note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns>The note's name.  For example, note 60 is "C4", and note 61 is "C#4".</returns>
        /// <exception cref="ArgumentOutOfRangeException">The note is out-of-range.</exception>
        public static string Name(this Note note)
        {
            note.Validate();
            return note.Family().Name() + note.Octave().ToString();
        }

        /// <summary>
        /// Returns the family of the specified note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns>The note's family.  For example, C4's family is C.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The note is out-of-range.</exception>
        public static NoteFamily Family(this Note note)
        {
            note.Validate();
            return (NoteFamily)((int)note % 12);
        }

        /// <summary>
        /// Returns the number of semitones that note is above tonic.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="tonic">The tonic.</param>
        /// <returns>The number of semitones, always in [0..11].</returns>
        public static int SemitonesAbove(this Note note, NoteFamily tonic)
        {
            int result = ((int)note % 12 - (int)tonic);
            if (result < 0)
            {
                result += 12;
            }
            return result;
        }
    }

    /// <summary>
    /// MIDI Note Families.
    /// </summary>
    /// <remarks>
    /// <para>A note's "family" is simply the note's name without regard to the specific octave.
    /// So, for example, CSharp3, CSharp4, and CSharp5 are all part of the CSharp note family.
    /// This enum lets us talk about the function or role of a note independent of the
    /// particular octave.
    /// </para>
    /// <para>This enum has extension methods, such as <see cref="NoteFamilyExtensionMethods.Name"/>
    /// and <see cref="NoteFamilyExtensionMethods.IsValid"/>, defined in
    /// <see cref="NoteFamilyExtensionMethods"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="Note"/>
    public enum NoteFamily
    {
        /// <summary>Note Family "C"</summary>
        C = 0,
        /// <summary>Note Family "C#"</summary>
        CSharp = 1,
        /// <summary>Note Family "D"</summary>
        D = 2,
        /// <summary>Note Family "D#"</summary>
        DSharp = 3,
        /// <summary>Note Family "E"</summary>
        E = 4,
        /// <summary>Note Family "F"</summary>
        F = 5,
        /// <summary>Note Family "F#"</summary>
        FSharp = 6,
        /// <summary>Note Family "G"</summary>
        G = 7,
        /// <summary>Note Family "G#"</summary>
        GSharp = 8,
        /// <summary>Note Family "A"</summary>
        A = 9,
        /// <summary>Note Family "A#"</summary>
        ASharp = 10,
        /// <summary>Note Family "B"</summary>
        B = 11
    }

    /// <summary>
    /// Extension methods for the NoteFamily enum.
    /// </summary>
    public static class NoteFamilyExtensionMethods
    {
        /// <summary>
        /// Returns true if the specified note family is valid.
        /// </summary>
        /// <param name="family">The family to test.</param>
        public static bool IsValid(this NoteFamily family)
        {
            return (int)family >= 0 && (int)family < 12;
        }

        /// <summary>
        /// Returns a copy of family which is made valid by wrapping if necessary.
        /// </summary>
        /// <param name="family">The family to wrap.</param>
        /// <returns>The wrapped copy of family.</returns>
        public static NoteFamily Wrapped(this NoteFamily family)
        {
            if ((int)family < 0)
            {
                return (NoteFamily)(11 - ((-(int)family - 1) % 12));
            }
            else
            {
                return (NoteFamily)((int)family % 12);
            }
        }

        /// <summary>
        /// Throws an exception if family is not valid.
        /// </summary>
        /// <param name="family">The family to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException">The family is out-of-range.</exception>
        public static void Validate(this NoteFamily family)
        {
            if (!family.IsValid())
            {
                throw new ArgumentOutOfRangeException("NoteFamily out of range");
            }
        }

        /// <summary>
        /// Table of note family names.
        /// </summary>
        private static string[] NoteFamilyNames =
            new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        /// <summary>
        /// Returns the human-readable name of a note family.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <exception cref="ArgumentOutOfRangeException">The family is out-of-range.</exception>
        public static string Name(this NoteFamily family)
        {
            family.Validate();
            return NoteFamilyNames[(int)family];
        }
    }
}
