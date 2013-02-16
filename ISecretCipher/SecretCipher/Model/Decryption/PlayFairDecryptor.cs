﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecretCipher.Model.Interfaces;
using SecretCipher.Model.Keys;
using SecretCipher.Utilities;

namespace SecretCipher.Model.Decryption
{
    public class PlayFairDecryptor : IASCIIDecryptor
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public PlayFairKey Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayFairDecryptor"/> class.
        /// </summary>
        /// <param name="p_key">The p_key.</param>
        public PlayFairDecryptor(PlayFairKey p_key)
        {

            this.Key = p_key;
           
        }
        private position GetPositionInTheMatrix(byte character)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {

                    if (Key.KeyMatrix[i, j] == character || (character == 74 && Key.KeyMatrix[i, j] == 73))
                        return new position(i, j);
                }
            }
            return new position(-1, -1);
        }
        private void FillDiagramsPositions(ref Diagram[]diagrams)
        {
            for (int i = 0; i < diagrams.Length; i++)
            {
                diagrams[i].firstCharacterPosition = GetPositionInTheMatrix(diagrams[i].firstCharacter);
                diagrams[i].secondCharacterPosition = GetPositionInTheMatrix(diagrams[i].secondCharacter);

                if (diagrams[i].firstCharacterPosition.row == diagrams[i].secondCharacterPosition.row)
                    diagrams[i].relationBetweenTheTwoCharacters = Relation.Row;
                else if (diagrams[i].firstCharacterPosition.column == diagrams[i].secondCharacterPosition.column)
                    diagrams[i].relationBetweenTheTwoCharacters = Relation.Column;
                else
                    diagrams[i].relationBetweenTheTwoCharacters = Relation.Nothing;

            }
        }
        private void BuildMatrix(ref byte[,] matrix)
        {
            bool[] AlphabitIsTaken = new bool[26];
            AlphabitIsTaken[74 - 65] = true;
            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (index < Key.Keyword.Length)
                    {
                        if ((AlphabitIsTaken[Key.Keyword[index] - 65] == false))
                        {
                            matrix[i, j] = (byte)Key.Keyword[index];
                            AlphabitIsTaken[Key.Keyword[index] - 65] = true;
                            index++;
                        }





                    }
                    else
                    {
                        for (int k = 0; k < 26; k++)
                        {
                            if (AlphabitIsTaken[k] == false)
                            {
                                matrix[i, j] = (byte)(k + 65);
                                AlphabitIsTaken[k] = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        private Diagram[] ToDiagrams(byte[] plainText)
        {


            //if (plainText.Length % 2 != 0)
            //{
            //plainText=  plainText.Concat( new byte[] {(byte)'X'}).ToArray();
            //}


            Diagram[] diagrams = new Diagram[plainText.Length / 2];
            int diagramsIndex = 0;
            for (int i = 0; i < plainText.Length; i += 2, diagramsIndex++)
            {
                if (i == plainText.Length - 1 && plainText.Length % 2 != 0)
                {
                    if (diagramsIndex < diagrams.Length)
                    {
                        diagrams[diagramsIndex].firstCharacter = plainText[i];

                        diagrams[diagramsIndex].secondCharacter = 'X' + 0;
                    }
                    else
                    {
                        Diagram d = new Diagram();
                        d.firstCharacter = plainText[i];

                        d.secondCharacter = 'X' + 0;
                        diagrams = diagrams.Concat(new Diagram[] { d }).ToArray();
                    }
                }
                else if (plainText[i] == plainText[i + 1])
                {
                    if (diagramsIndex < diagrams.Length)
                    {
                        diagrams[diagramsIndex].firstCharacter = plainText[i];

                        diagrams[diagramsIndex].secondCharacter = 'X' + 0;
                        i--;
                    }
                    else
                    {
                        Diagram d = new Diagram();
                        d.firstCharacter = plainText[i];

                        d.secondCharacter = 'X' + 0;
                        i--;
                        diagrams = diagrams.Concat(new Diagram[] { d }).ToArray();
                    }
                }
                else
                {
                    if (diagramsIndex < diagrams.Length)
                    {

                        diagrams[diagramsIndex].firstCharacter = plainText[i];
                        if (i + 1 < plainText.Length)
                            diagrams[diagramsIndex].secondCharacter = plainText[i + 1];
                        else
                            diagrams[diagramsIndex].secondCharacter = 'X' + 0;
                    }
                    else
                    {
                        Diagram d = new Diagram();
                        d.firstCharacter = plainText[i];
                        if (i + 1 < plainText.Length)
                            d.secondCharacter = plainText[i + 1];
                        else
                            d.secondCharacter = 'X' + 0;
                        diagrams = diagrams.Concat(new Diagram[] { d }).ToArray();
                    }
                }
            }



            FillDiagramsPositions(ref diagrams);
            return diagrams;

        }
        private byte GetRowAndColumnLetter(position position)
        {
            return Key.KeyMatrix[position.row, position.column];
        }
        private byte GetAboveLetter(position position)
        {
            if ((position.row - 1) < 0)
                return Key.KeyMatrix[4, position.column];
            else
                return Key.KeyMatrix[(position.row - 1), position.column];

        }
        private byte GetLeftLetter(position position)
        {
            if ((position.column - 1) < 0)
                return Key.KeyMatrix[position.row, 4];
            else
                return Key.KeyMatrix[position.row, (position.column - 1) % 5];
        }
        private byte[] StringToByteArray(string text)
        {
            byte[] textInByes = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                textInByes[i] = (byte)text[i];
            }
            return textInByes;

        }
        private double[,] ByteToDoubleArray(byte[,] byteArray)
        {

            int rows = byteArray.GetLength(0);
            int columns = byteArray.GetLength(1);
            double[,] doubleArray = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    doubleArray[i, j] = byteArray[i, j];
                }
            }
            return doubleArray;
        }
        private string ByteToString(byte[] arr)
        {
            string s = "";
            for (int i = 0; i < arr.Length; i++)
                s += (char)arr[i];
            return s;

        }
        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <param name="p_cipherText">The p_cipher text.</param>
        /// <returns></returns>
        public string DecryptMessage(string p_cipherText)
        {
            p_cipherText = p_cipherText.ToUpper();
            byte[]cipherText = StringToByteArray(p_cipherText);
            byte[] plainText = new byte[p_cipherText.Length];
         
            
           Diagram[] diagrams= ToDiagrams( cipherText);
            for (int currentDiagramIndex = 0; currentDiagramIndex < diagrams.Length; currentDiagramIndex++)
            {
                if (diagrams[currentDiagramIndex].relationBetweenTheTwoCharacters == Relation.Row)
                {
                    plainText[currentDiagramIndex * 2] = GetLeftLetter(diagrams[currentDiagramIndex].firstCharacterPosition);
                    plainText[currentDiagramIndex * 2 + 1] = GetLeftLetter(diagrams[currentDiagramIndex].secondCharacterPosition);
                }
                else if (diagrams[currentDiagramIndex].relationBetweenTheTwoCharacters == Relation.Column)
                {
                    plainText[currentDiagramIndex * 2] = GetAboveLetter(diagrams[currentDiagramIndex].firstCharacterPosition);
                    plainText[currentDiagramIndex * 2 + 1] = GetAboveLetter(diagrams[currentDiagramIndex].secondCharacterPosition);
                }
                else
                {
                    plainText[currentDiagramIndex * 2] = GetRowAndColumnLetter(new position(diagrams[currentDiagramIndex].firstCharacterPosition.row, diagrams[currentDiagramIndex].secondCharacterPosition.column));

                    plainText[currentDiagramIndex * 2 + 1] = GetRowAndColumnLetter(new position(diagrams[currentDiagramIndex].secondCharacterPosition.row, diagrams[currentDiagramIndex].firstCharacterPosition.column));


                }
            }
            return ByteToString(plainText);
        }
    }
}
