namespace AspektML.AI
{
    public struct  Move
    {
        public Pawn pawn;
        public Tile tile;
        public float weight;

        public Move(Pawn pawn, Tile tile)
        {
            this.pawn = pawn;
            this.tile = tile;
            weight = 1f;
        }
    }
}
