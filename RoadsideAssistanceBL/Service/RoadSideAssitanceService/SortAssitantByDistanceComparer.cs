using RoadsideAssistanceBL.Models;

namespace RoadsideAssistanceBL.Service
{
    public class SortAssitantByDistanceComparer : IComparer<Assistant>
    {
        private readonly Geolocation location;
        public SortAssitantByDistanceComparer(Geolocation location)
        {
            this.location = location;
        }

        public int Compare(Assistant x, Assistant y)
        {
            var assitantXDistance = GetDistance(x);
            var assitantYDistance = GetDistance(y);
            if (assitantXDistance < assitantYDistance)
            {
                return -1;
            }
            else if (assitantXDistance > assitantYDistance)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private double GetDistance(Assistant assistant)
        {
            //distance formula
            // distance = Sqrt(power(X2-X1)^2 + power(Y2-Y1)^2)
            var distance = Math.Sqrt(Math.Pow(assistant.GetLocation().X - location.X, 2) +
                                     Math.Pow(assistant.GetLocation().Y - location.Y, 2));

            return distance;
        }
    }
}
