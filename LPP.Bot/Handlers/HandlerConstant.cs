namespace LPP.Bot.Handlers
{
    public class HandlerConstant
    {
        public static string AdminMenu = "⚙️ Администрирование 🔧";

        public static string ShowProgram = "📅 Программа конкурса";

        public static string DeleteBook = "🗑 Удалить книгу";

        public static string ShowSections = "Разделы";

        public static string AddBookCmd { get; internal set; } = "AddBookCmd";
        public static string AddSectionCmd { get; internal set; } = "AddSectionCmd";
        public static string GetUsersCmd { get; internal set; } = "GetUsersCmd";
        public static string SetAdminCmd { get; internal set; } = "SetAdminCmd";

        public static string Support { get; internal set; } = "💬 Задать вопрос";
        public static string DeleteBookQuery { get; internal set; } = "delete_book";
        public static string DeleteBookConfirmQuery { get; internal set; } = "delete__book__confirm";

        public static string DeleteBookCancelQuery { get; internal set; } = "delete__book__cancel";
        public static string EditBook { get; internal set; } = "✏ Редактировать книгу";
        public static string EditBookQuery { get; internal set; } = "edit_book";
        public static string EditBookProcess { get; internal set; } = "edit__book__process";
        public static string SendAdminPinQuery { get; internal set; } = "sendAdminPin";

        public static string ShowBookPagingQuery { get; internal set; } = "showBookPaging";

        public static string MailingQuery { get; internal set; } = "mailing";

        public static string BooksCommand { get; internal set; } = "/books";

        public static string SupportCommand { get; internal set; } = "/support";
        public static string AddSectionQuery { get; internal set; } = "addSectionQuery";
        public static string ShowSectionQuery { get; internal set; } = "showSectionQuery";
        public static string ExcelBookListQuery { get; internal set; } = "ExcelBookListQuery";
        public static string Byliner { get; internal set; } = "Byliner";
        public static string AboutCompetition { get; internal set; } = "AboutLpp";
        public static string? ShowNews { get; internal set; } = "📰 Новости конкурса";
        public static string ShowSchemes { get; internal set; } = "📜 Схемы конкурса";
    }
}
