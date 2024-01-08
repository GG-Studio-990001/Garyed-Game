using System;

namespace Runtime.Data.Original
{
    [Serializable]
    public class Quarter : ICloneable
    {
        public int chapter;
        public int stage;
        public int minor;

        public object Clone()
        {
            return new Quarter
            {
                chapter = chapter,
                stage = stage,
                minor = minor
            };
        }
    }
}