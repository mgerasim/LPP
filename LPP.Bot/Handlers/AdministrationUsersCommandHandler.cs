using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LPP.Bot.Handlers
{
    public class AdministrationUsersCommand : IRequest
    {

    }

    public class AdministrationUsersCommandHandler : IRequestHandler<AdministrationUsersCommand>
    {
        private readonly LPPContext context;
        private readonly KeyboardHandler keyboardHandler;
        private readonly CurrentUserState userState;

        public AdministrationUsersCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }
        public async Task Handle(AdministrationUsersCommand request, CancellationToken cancellationToken)
        {
            this.userState.UserState.State = string.Empty;

            this.userState.UserState.CurrentStep = string.Empty;

            var users = await this.context.Users
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            ExcelPackage.License.SetNonCommercialPersonal("LPP");

            // Создание Excel-файла
            using var package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("Список пользователей");

            int row = 1;

            // Добавление наименования отчёта
            worksheet.Cells[row, 1, row, 8].Merge = true;
            worksheet.Cells[row, 1, row, 8].Value = "Список пользователей";
            worksheet.Row(row).Style.Font.Bold = true;
            worksheet.Row(row).Style.Font.Size = worksheet.Row(row).Style.Font.Size * 1.5f;
            worksheet.Row(row).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            row++;
            // Заголовки
            worksheet.Cells[row, 1].Value = "№";
            worksheet.Cells[row, 2].Value = "Дата создания";
            worksheet.Cells[row, 3].Value = "Дата последнего входа";
            worksheet.Cells[row, 4].Value = "Идентификатор";
            worksheet.Cells[row, 5].Value = "Логин";
            worksheet.Cells[row, 6].Value = "Имя";
            worksheet.Cells[row, 7].Value = "Фамилия";
            worksheet.Cells[row, 8].Value = "Админ?";
            worksheet.Row(row).Style.Font.Bold = true;
            worksheet.Cells[row, 1, row, 8].AutoFilter = true;

            // Добавление данных
            row++;
            foreach (var u in users)
            {
                Models.Entities.User user = u;

                worksheet.Cells[row, 1].Value = user.Id;
                worksheet.Cells[row, 2].Value = user.CreatedAt;
                worksheet.Cells[row, 2].Style.Numberformat.Format = "yyyy-MM-dd HH:mm";

                worksheet.Cells[row, 3].Value = user.UpdatedAt;
                worksheet.Cells[row, 3].Style.Numberformat.Format = "yyyy-MM-dd HH:mm";

                worksheet.Cells[row, 4].Value = user.TelegramId;
                worksheet.Cells[row, 5].Value = user.Username;
                worksheet.Cells[row, 6].Value = user.FirstName;
                worksheet.Cells[row, 7].Value = user.LastName;
                worksheet.Cells[row, 8].Value = user.IsAdmin ? "Да" : "Нет";

                worksheet.Row(row).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Row(row).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                row++;
            }

            // Автоширина колонок
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Сохранение файла в поток
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            // Отправка файла через Telegram-бота
            var fileName = "Список сотрудников.xlsx";

            var inputFile = InputFile.FromStream(stream, fileName);
            await this.userState.BotClient.SendDocument(this.userState.ChatId, inputFile, caption: "Список сотрудников во вложении");
        }
    }
}
