using System.ComponentModel.DataAnnotations.Schema;

namespace Module.CrossCutting
{
    public interface IObjectState
    {
        [NotMapped] ObjectState ObjectState { get; set; }
    }

    public enum ObjectState
    {
        Unchanged,
        Added,
        Modified,
        Deleted
    }
}