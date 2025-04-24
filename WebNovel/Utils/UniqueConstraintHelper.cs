using Microsoft.EntityFrameworkCore.Metadata;

namespace WebNovel.Utils
{
    public class UniqueConstraintHelper
    {
        private readonly IModel _model;

        public UniqueConstraintHelper(IModel model)
        {
            _model = model;
        }

        public Dictionary<Type, List<string>> GetAllUniqueFields()
        {
            var result = new Dictionary<Type, List<string>>();

            foreach (var entityType in _model.GetEntityTypes())
            {
                var uniqueFields = entityType.GetIndexes()
                    .Where(i => i.IsUnique)
                    .SelectMany(i => i.Properties.Select(p => p.Name))
                    .Distinct()
                    .ToList();

                if (uniqueFields.Any())
                    result[entityType.ClrType] = uniqueFields;
            }

            return result;
        }
    }

}
