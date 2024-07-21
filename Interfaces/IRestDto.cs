using StudentSystem.Dto;
namespace StudentSystem {
    namespace Interfaces {
        //Define an interface for DTOs to include HATEAOS support.
        public interface IRestDto {

            /// <summary>
            /// HATEOAS Links.
            /// </summary>
            public List<LinkDto>? Links { get; set; }
        }
    }
}
