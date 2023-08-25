using SalesTracker.Data;

namespace SalesTracker.Utility
{
    public class PageHitsTracker
    {

        public void AddPageHit(SalesTrackerDBContext context)
        {
            var hit = context.PageHits.Where(w => w.Date.Date == DateTime.Now.Date).FirstOrDefault();
            if(hit == null)
            {
                hit = new Models.PageHits();
                hit.Date = DateTime.Now.Date;
                hit.Hits = 1;

                context.PageHits.Add(hit);
            }
            else
            {
                hit.Hits++;
            }

            context.SaveChanges();

        }
    }
}
