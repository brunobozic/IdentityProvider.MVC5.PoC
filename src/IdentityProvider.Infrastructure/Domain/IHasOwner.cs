namespace IdentityProvider.Infrastructure.Domain
{
    public interface IHasOwner
    {
        /// <summary>
        /// The object instance this object belongs to.
        /// </summary>
        object Owner { get; set; }
    }
}
