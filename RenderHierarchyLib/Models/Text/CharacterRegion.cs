namespace RenderHierarchyLib.Render.Text
{
    public struct CharacterRegion
    {
        public char Start;

        public char End;

        public int StartIndex;

        public CharacterRegion(char start, int startIndex)
        {
            Start = start;
            End = start;
            StartIndex = startIndex;
        }
    }
}
