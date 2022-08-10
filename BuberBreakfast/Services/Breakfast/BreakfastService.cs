using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Services.Breakfast;

public interface IBreakfastService
{
    ErrorOr<Created> CreateBreakfast(Models.Breakfast breakfast);
    ErrorOr<Models.Breakfast> GetBreakfast(Guid id);
    Dictionary<Guid, Models.Breakfast> GetAllBreakfasts();
    ErrorOr<Deleted> DeleteBreakfast(Guid id);
    ErrorOr<UpsertedBreakfast> UpsertBreakfast(Models.Breakfast breakfast);
}

public class BreakfastService : IBreakfastService
{
    public static Dictionary<Guid, Models.Breakfast> breakfasts = new Dictionary<Guid, Models.Breakfast>();
    public ErrorOr<Created> CreateBreakfast(Models.Breakfast breakfast)
    {
        breakfasts.Add(breakfast.Id, breakfast);

        return Result.Created;
    }

    public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    {
        breakfasts.Remove(id);

        return Result.Deleted;
    }

    public Dictionary<Guid, Models.Breakfast> GetAllBreakfasts()
    {
        return breakfasts;
    }

    public ErrorOr<Models.Breakfast> GetBreakfast(Guid id)
    {
        if (breakfasts.TryGetValue(id, out var breakfast))
        {
            return breakfast;
        }

        return Errors.Breakfast.NotFound;
    }

    public ErrorOr<UpsertedBreakfast> UpsertBreakfast(Models.Breakfast breakfast)
    {
        var IsNewlyCreated = !breakfasts.ContainsKey(breakfast.Id);
        breakfasts[breakfast.Id] = breakfast;

        return new UpsertedBreakfast(IsNewlyCreated);
    }
}
