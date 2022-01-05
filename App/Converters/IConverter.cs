namespace App.Converters
{
    public interface IConverter<Tdb, Tui, T>
    {
        Tui ToUi(Tdb db);
        T ToDomainClass(Tdb db);
    }
}