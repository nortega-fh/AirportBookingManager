namespace AirportBooking.Filters;

public interface IFilter<Model, FilterParams>
    where Model : class?
    where FilterParams : class?
{
    IReadOnlyList<Model> SearchByParameters(FilterParams filterParams, IList<Model> originalList);
}
