using CTHelper.Application.Models.Image;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;

namespace CTHelper.Application.Specification.ImageSpecifications
{
    public class GetImageCredsByImageIdSpecification : BaseSpecification<Image, ImageCredsModel>
    {
        public GetImageCredsByImageIdSpecification(long imageId)
        {
            AddCriteria(i => i.Id == imageId);
            ApplySelector(i => new ImageCredsModel
            {
                Bucket = i.Bucket,
                ObjectKey = i.ObjectKey
            });
        }
    }
}
