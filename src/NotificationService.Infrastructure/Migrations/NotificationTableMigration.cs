using FluentMigrator;

namespace NotificationService.Infrastructure.Migrations;

[Migration(000001)]
public class NotificationTableMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Notifications")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("Recipient").AsFixedLengthString(128).NotNullable()
            .WithColumn("Message").AsFixedLengthString(32676).NotNullable()
            .WithColumn("NotificationChannel").AsFixedLengthString(32).NotNullable()
            .WithColumn("NotificationStatus").AsFixedLengthString(32).NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("Errors").AsString().Nullable()
            .WithColumn("SentAt").AsDateTime().Nullable();
    }
}