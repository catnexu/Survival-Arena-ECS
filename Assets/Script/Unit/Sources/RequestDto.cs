namespace Unit
{
    public struct RequestDto
    {
        public string Id { get; }
        public int OwnerLayer { get; }
        public int TargetLayerMask { get; }

        public RequestDto(string id, int ownerLayer, int targetLayerMask)
        {
            Id = id;
            OwnerLayer = ownerLayer;
            TargetLayerMask = targetLayerMask;
        }
    }
}