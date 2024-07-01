namespace Application.Common.Security.Permissions;

public static partial class Reminder
{
    public static class Item
    {
        public const string Read = "read:item";
        public const string Create = "create:item";
        public const string Edit = "edit:item";
        public const string Delete = "delete:item";
    }
}
