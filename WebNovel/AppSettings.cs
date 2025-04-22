namespace WebNovel
{
    public class AppSettings
    {
        public string ProjectDefault { get; set; }
        public string SiteURL { get; set; }
        public string JWTAuthenticateSecret { get; set; }
        public int JWTAuthenticateExpiresHours { get; set; }
        public int CommentPageSize { get; set; }
        public int FollowersPageSize { get; set; }
        public int GroupPageSize { get; set; }
        public int NotificationsPageSize { get; set; }
        public int LengthOfNotificationCharacter { get; set; }
        public int PeoplePageSize { get; set; }
        public int FriendsPageSize { get; set; }

        //public static Stream LStream => new MemoryStream(Convert.FromBase64String("PExpY2Vuc2U+CjxEYXRhPgo8TGljZW5zZWRUbz5Bc3Bvc2UgUHR5IEx0ZDwvTGljZW5zZWRUbz4KPEVtYWlsVG8+c2FsbWFuLnNhcmZyYXpAYXNwb3NlLmNvbTwvRW1haWxUbz4KPExpY2Vuc2VUeXBlPkRldmVsb3BlciBTbWFsbCBCdXNpbmVzczwvTGljZW5zZVR5cGU+CjxMaWNlbnNlTm90ZT5PbmUgRGV2ZWxvcGVyIEFuZCBPbmUgRGVwbG95bWVudCBMb2NhdGlvbjwvTGljZW5zZU5vdGU+CjxPcmRlcklEPjIwMDUwNjA3NTkzMzwvT3JkZXJJRD4KPFVzZXJJRD43NDc0MTU8L1VzZXJJRD4KPE9FTT5UaGlzIGlzIG5vdCBhIHJlZGlzdHJpYnV0YWJsZSBsaWNlbnNlPC9PRU0+CjxQcm9kdWN0cz4KPFByb2R1Y3Q+Q29uaG9sZGF0ZS5Ub3RhbCBmb3IgLk5FVDwvUHJvZHVjdD4KPC9Qcm9kdWN0cz4KPEVkaXRpb25UeXBlPlByb2Zlc3Npb25hbDwvRWRpdGlvblR5cGU+CjxTZXJpYWxOdW1iZXI+ODUxZTUwM2MtYmU0NS00M2I4LWFjZWItMzM4OGNmMWQxY2Y1PC9TZXJpYWxOdW1iZXI+CjxTdWJzY3JpcHRpb25FeHBpcnk+MjAyMTA1MDY8L1N1YnNjcmlwdGlvbkV4cGlyeT4KPExpY2Vuc2VWZXJzaW9uPjMuMDwvTGljZW5zZVZlcnNpb24+CjxMaWNlbnNlSW5zdHJ1Y3Rpb25zPmh0dHBzOi8vcHVyY2hhc2UuY29uaG9sZGF0ZS5jb20vcG9saWNpZXMvdXNlLWxpY2Vuc2U8L0xpY2Vuc2VJbnN0cnVjdGlvbnM+CjwvRGF0YT4KPFNpZ25hdHVyZT5Na2xtMDFpbnZXZGdEN0s0MlR1RUJpejF6VUZIZUdSalZaOWQ4REg3cnFQa1dqM1gwMXovOGM5QkI2MG9FL2lhemRHUjExY0VsUkdSVG51SER5SUJITlhBcUFSNnZqYysxMW9OZXdpSW80ZTVoODhGMm0vdVE0Yzd3VUFZemVLbldQc0FISU1XU0FzNGE1WW5iZVpVK3dad1cxaUdkZzRGWlBFUnU2bFdNYzg9PC9TaWduYXR1cmU+CjwvTGljZW5zZT4="));
        //public static void SetAsposeLicense()
        //{
        //    new Aspose.Words.License().SetLicense(LStream);
        //    new Aspose.Cells.License().SetLicense(LStream);
        //}
    }
}
