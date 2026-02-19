namespace CTHelper.Persistence.Common.Enums
{
    public enum UserEventTypeEnum : short
    {
        Login = 1,
        Logout = 2,
        TestStarted = 3,
        TestFinished = 4,
        TestAbandoned = 5,
        WasJoinedToGroup = 6,
        WasExcludedFromGroup = 7,
        FavoriteProblemAdded = 8,
        FavoriteProblemRemoved = 9,
        FavoriteTestAdded = 10,
        FavoriteTestRemoved = 11
    }
}
