using StudentSystem.Interfaces;

namespace StudentSystem.Dto {
    /// <summary>
    /// Defines a paged collection result for specified IRestBaseDto
    /// </summary>
    public record PagedCollectionResultDto<T> : RestBaseDto, IRestResponseDto where T : RestBaseDto {
        
        public List<T>? Results { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? PageCount { get; set; }

        public int? ResultCount { get; set; }
        public int? TotalCount { get; set; }

        public List<LinkDto>? Links { get; set; }
    }
}
