using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretCipher.Utilities
{
    public enum Relation { Row, Column, Nothing }

    public struct position
    {
        public position(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
        public int row;
        public int column;
    }

    public struct Diagram
    {
        public byte firstCharacter;
        public position firstCharacterPosition;
        public byte secondCharacter;
        public position secondCharacterPosition;
        public Relation relationBetweenTheTwoCharacters;
    }

}
