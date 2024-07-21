namespace StudentSystem.Interfaces {
    public interface ILinkDto {
        /// <summary>
        /// Url for the resource action.
        /// </summary>
        public string? Href { get; set; }

        /// <summary>
        /// Relationship of the link.
        /// </summary>
        public string? Rel { get; set; }

        /// <summary>
        /// Http Method for the action.
        /// </summary>
        public string? Type { get; set; }
    }
}
