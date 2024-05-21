using NoireRules.Models;
using System.ComponentModel.DataAnnotations;

namespace NoireServer.Database.Model
{
    public class PlayerData
    {
        [Key] public int Id { get; set; }
        public string? Name { get; set; } = "";
        public int GameId { get; set; }
        public virtual GameData? Game { get; set; }
        public string? IdentityForInspector { get; set; } = "";
        public Role YourRole { get; set; }
        public bool IsYourMove { get; set; }
    }
}
