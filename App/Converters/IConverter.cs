namespace App.Converters
{
    public interface IConverter<in TDb, out TUi, out T>
    {
        TUi ToUi(TDb db);
        T ToDomainClass(TDb db);
    }
}