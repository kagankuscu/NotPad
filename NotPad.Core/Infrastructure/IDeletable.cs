namespace NotPad.Core.Infrastructure
{
    internal interface IDeletable
    {
        bool IsDeleted { get; set; }
    }
}