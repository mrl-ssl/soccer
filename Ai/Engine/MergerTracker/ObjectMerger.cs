
using MRL.SSL.Common;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Utils.Extensions;

namespace MRL.SSL.Ai.MergerTracker
{
    internal class ObjectMerger
    {
        int affinity;
        Observation[] observations;

        public int Affinity { get => affinity; set => affinity = value; }
        public Observation[] Observations { get => observations; set => observations = value; }
        public ObjectMerger()
        {
            observations = new Observation[MergerTrackerConfig.Default.MaxCameraCount].Populate((i) => new Observation((uint)i));
            affinity = -1;
        }
        public int MergeObservations()
        {
            int lastAffinity = affinity;
            var config = MergerTrackerConfig.Default;
            if (affinity < 0 || !observations[affinity].IsValid)
            {

                affinity = -1;
                float minDist = float.MaxValue;
                for (int o = 0; o < config.MaxCameraCount; o++)
                {
                    if (observations[o].IsValid)
                    {
                        if (lastAffinity >= 0)
                        {
                            float d = observations[lastAffinity].Location.Distance(observations[o].Location);
                            if (d < minDist)
                            {
                                minDist = d;
                                affinity = o;
                            }
                        }
                        else
                        {
                            affinity = o;
                            break;
                        }
                    }
                }
            }

            if (affinity < 0 && lastAffinity >= 0 && observations[lastAffinity].LastValid < config.AffinityPersist)
            {
                affinity = lastAffinity;
            }


            return affinity;
        }
        public int MergeObservations(int ballAffinity)
        {
            int lastAffinity = affinity;
            var config = MergerTrackerConfig.Default;

            if (ballAffinity >= 0 && affinity != ballAffinity && observations[ballAffinity].IsValid)
                affinity = ballAffinity;
            else if (affinity < 0 || !observations[affinity].IsValid)
            {

                affinity = -1;
                float minDist = float.MaxValue;
                for (int o = 0; o < config.MaxCameraCount; o++)
                {
                    if (observations[o].IsValid)
                    {
                        if (lastAffinity >= 0)
                        {
                            float d = observations[lastAffinity].Location.Distance(observations[o].Location);
                            if (d < minDist)
                            {
                                minDist = d;
                                affinity = o;
                            }
                        }
                        else
                        {
                            affinity = o;
                            break;
                        }

                    }
                }
            }

            if (affinity < 0 && lastAffinity >= 0 && observations[lastAffinity].LastValid < config.AffinityPersist)
            {
                affinity = lastAffinity;
            }

            return affinity;
        }

    }
}