namespace Unit
{
    public struct RequestDto
    {
        public string Id { get; }

        public RequestDto(string id)
        {
            Id = id;
        }
    }
}