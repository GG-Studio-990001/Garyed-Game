using System;

namespace Runtime.Data
{
    [Serializable]
    public class Quarter : ICloneable
    {
        public int chapter;
        public int major;
        public int minor;

        public object Clone()
        {
            return new Quarter
            {
                chapter = chapter,
                major = major,
                minor = minor
            };
        }
    }
}