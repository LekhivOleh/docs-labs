using System.ComponentModel.DataAnnotations;

namespace docs_project.Application.Dto.Upsert
{
    public class GroupChatUpsertDto : ChatUpsertDtoBase
    {
        [Required]
        public string? Title { get; set; }
        public Guid OwnerId { get; set; }
        public List<Guid> UserIds { get; set; } = new();
    }

    public class DirectChatUpsertDto : ChatUpsertDtoBase
    {
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
    }
}

