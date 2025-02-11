using FluentMigrator;

namespace MessageService.Migrations;

[Migration(1)]
public class CreateMessagesTable : Migration
{
    public override void Up()
    {
        Create.Table("messages")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("text").AsString(128).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("sequence_number").AsInt32().NotNullable();
    }
        
    public override void Down()
    {
        Delete.Table("messages");
    }
}
