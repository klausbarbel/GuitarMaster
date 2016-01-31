﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Midi;
using System.Threading;
using MidiExamples;
using System.Windows.Media;
using System.IO;

/* Плюшка: можно выводить мелодию в виде текста. Получается её можно проиграть заново.
 * А еще можно создать базу данных и в нёё класть классные мелодии.
 * 
 * Следующая задача: определить, что будет хранить массив мелоди(сейчас он хранит номера ступеней от 1 до 7)
 * Добавить новую гамму. И прорисовать её на грифе
 * 
 * Мелодия.
 * Сделать рандомный ритм.(сделано) Также можно задавать темп. Попробовать сделать его тоже рандомным
 * Добавить приемы: секвенция, вертушка, арпеджио, повышение(понижение октавы)
 * 
 * Задача: переделать программу.
 * 1. Аккомпанемент. Будет играть один аккорд. Еще надо решить, 
 * будет ли мелодия зависеть от аккомпанемента(должна вообще то)
 * Для начала
 * запишем аккорд: минорный, мажорный. Пока что в одной тональности. Как быть для восточных ладов? Для блюза?
 * 2. Гаммы.
 *  2.1 Нужно, чтобы при создании мелодии брались ноты из гаммы.
 *  2.2 Сделать добавление своих ладов(на входе: количество нот, интервалы)
 *  2.3 Самому добавить блюзовую гамму, минорную мажорную, восточную
 * 3. Ритм. Сделать хоть какой-то римт
 *  3.1 Создание рандомного ритма
 *  3.2 Характерные ритмы для блюза, восточной музыки
 * 4. Мелодия. Как строится мелодия.
 *  Что на входе?
 *  Ритм - количество нот и пауз
 *  Набор нот. Количество должно совпадать естественно
 *  
 * 5. Сделать, чтобы мелодия игралась от тоники, выбранной на грифе. Т.е. мелодия
 * должна играться в любой тональности
 * */
namespace GuitarMaster
{
    public partial class Form1 : Form
    {
        public Button[,] buttons;
        OutputDevice outputDevice;
        public MediaPlayer player;

        public Form1()
        {
            InitializeComponent();
        }

        private void newGenerateButton_Click(object sender, EventArgs e)
        {
            int[] rhythm = Rhythm.GetRhythm(25, 16);
            int[] notes = Notes.GetNotes(Notes.Chords.Am, 1);
            //int[] notes = Notes.NewGetNotes(flamencoScale, 16, rhythm);

            SoundDevices sd = new SoundDevices(outputDevice, Channel.Channel1);
            
            MelodyPlayer.PlayMelodyWithRhythm(sd, notes, rhythm, Note.A4, 5);
        }

        public static void Replay(MediaPlayer player)
        {
            if (player.Source.ToString() == "file:///D:/Visual_Studio_Projects/GuitarMaster/GuitarMaster/bin/Debug/Chords/Am.m4a")
            {
                player.Open(new Uri(Application.StartupPath + "\\Chords\\F.m4a", UriKind.Absolute));
                player.Play();

                return;
            }
            if (player.Source.ToString() == "file:///D:/Visual_Studio_Projects/GuitarMaster/GuitarMaster/bin/Debug/Chords/F.m4a")
            {
                player.Open(new Uri(Application.StartupPath + "\\Chords\\Dm.m4a", UriKind.Absolute));
                player.Play();
                return;
            }
            if (player.Source.ToString() == "file:///D:/Visual_Studio_Projects/GuitarMaster/GuitarMaster/bin/Debug/Chords/Dm.m4a")
            {
                player.Open(new Uri(Application.StartupPath + "\\Chords\\E.m4a", UriKind.Absolute));
                player.Play();
                return;
            }
        }

        private void generateButton_Click(object sender, EventArgs e)
        {

            int[] notes = Notes.GetNotes(Notes.Chords.Am, 1);
            int[] rhythm = Rhythm.GetRhythm(6, 4);

            for (int i = 0; i < notes.Length; i++)
            {
                testTextBox.Text += notes[i].ToString();
            }
            testTextBox.Text += " ";
          
            player.Open(new Uri(Application.StartupPath + "\\Chords\\Am.m4a", UriKind.Absolute));
            player.Play();

            SoundDevices sd = new SoundDevices(outputDevice, Channel.Channel1);

            MelodyPlayer.PlayMelody(sd, notes, player);

            notes = Notes.GetNotes(Notes.Chords.F, 2);
            for (int i = 0; i < notes.Length; i++)
            {
                testTextBox.Text += notes[i].ToString();
            }
            testTextBox.Text += " ";

            MelodyPlayer.PlayMelody(sd, notes, player);

            notes = Notes.GetNotes(Notes.Chords.Dm, 2);
            for (int i = 0; i < notes.Length; i++)
            {
                testTextBox.Text += notes[i].ToString();
            }
            testTextBox.Text += " ";

            MelodyPlayer.PlayMelody(sd, notes, player);

            notes = Notes.GetNotes(Notes.Chords.E, 4);
            for (int i = 0; i < notes.Length; i++)
            {
                testTextBox.Text += notes[i].ToString();
            }
            testTextBox.Text += " ";

            MelodyPlayer.PlayMelody(sd, notes, player);
        }

        private void grifPBox_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    VisibleCoordButtons(buttons[i, j], e, grifPBox, i, j);
                }
            }

        }

        private void grif_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if ((Button)sender == buttons[i, j])
                    {
                        outputDevice.SendNoteOn(Channel.Channel1, grifnotes[i, j], 80);
                        if (!grid)
                        {
                            grid = true;
                            DrawGrid(i, j, patternComboBox.SelectedIndex);
                        }
                        return;
                    }
                }
            }
        }

        private void clearGrid_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    buttons[i, j].Visible = false;
                    gridButtons[i, j] = 0;
                    labels[i, j].Visible = false;
                }
            }

            grid = false;
        }

        private void accompButton_Click(object sender, EventArgs e)
        {
            Accompaniment.PlayAccompanement(outputDevice);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grifPBox_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), 50, 50, 50, 50);
            //for (int i = 0; i < 6; i++)
            //{
            //    for (int j = 0; j < 16; j++)
            //    {
            //        if (gridButtons[i, j] == 1)
            //        {
            //            int x = buttons[i, j].Location.X - grifPBox.Location.X;
            //            int y = buttons[i, j].Location.Y - grifPBox.Location.Y;
            //            e.Graphics.FillEllipse(new System.Drawing.SolidBrush(System.Drawing.Color.Red), x, y, 50, 50);
            //        }
            //    }
            //}
        }       

        //private void stopButton_Click(object sender, EventArgs e)
        //{
        //    //player.Stop();
        //    //outputDevice.Close();
        //}


    }
}
