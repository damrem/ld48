using Algorithms.Graphs;
using System;
using System.Collections.Generic;
using DataStructures.Graphs;

namespace Damrem.Graphs {

    public class Dijkstra<TGraph, TVertex>
      where TGraph : IGraph<TVertex>, IWeightedGraph<TVertex>
      where TVertex : IComparable<TVertex> {

        readonly Dictionary<TVertex, DijkstraShortestPaths<TGraph, TVertex>> dijkstraShortestPathsByVertex = new Dictionary<TVertex, DijkstraShortestPaths<TGraph, TVertex>>();
        readonly Dictionary<TVertex, Dictionary<TVertex, bool>> havePathByDestinationBySource = new Dictionary<TVertex, Dictionary<TVertex, bool>>();
        readonly Dictionary<TVertex, Dictionary<TVertex, long>> distancesByDestinationBySource = new Dictionary<TVertex, Dictionary<TVertex, long>>();
        readonly Dictionary<TVertex, Dictionary<TVertex, IEnumerable<TVertex>>> shortestPathsByDestinationBySource = new Dictionary<TVertex, Dictionary<TVertex, IEnumerable<TVertex>>>();

        TGraph Graph { get; set; }

        public Dijkstra(TGraph graph) {
            Graph = graph;
        }

        DijkstraShortestPaths<TGraph, TVertex> GetDijkstraShortestPaths(TVertex from) {
            if (!dijkstraShortestPathsByVertex.TryGetValue(from, out var dijkstraShortestPaths)) {
                dijkstraShortestPaths = new DijkstraShortestPaths<TGraph, TVertex>(Graph, from);
                dijkstraShortestPathsByVertex.Add(from, dijkstraShortestPaths);
            }
            return dijkstraShortestPaths;
        }

        public delegate T Computer<T>(DijkstraShortestPaths<TGraph, TVertex> dsp, TVertex destination);

        T ComputeAndCacheByDestination<T>(TVertex source, TVertex destination, Computer<T> computer, Dictionary<TVertex, T> cacheByDestination) {
            var dsp = GetDijkstraShortestPaths(source);
            var computed = computer.Invoke(dsp, destination);
            cacheByDestination.Add(destination, computed);
            return computed;
        }

        T ComputeAndCache<T>(TVertex source, TVertex destination, Computer<T> computer, Dictionary<TVertex, Dictionary<TVertex, T>> cacheByDestinationBySource) {
            var dsp = GetDijkstraShortestPaths(source);
            T computed;
            if (!cacheByDestinationBySource.TryGetValue(source, out var cacheByDestination)) {
                cacheByDestination = new Dictionary<TVertex, T>();
                computed = ComputeAndCacheByDestination(source, destination, computer, cacheByDestination);
                cacheByDestinationBySource.Add(source, cacheByDestination);
            }
            else if (!cacheByDestination.TryGetValue(destination, out computed)) {
                computed = ComputeAndCacheByDestination(source, destination, computer, cacheByDestination);
            }
            return computed;
        }

        public bool HasPath(TVertex source, TVertex destination) {
            return ComputeAndCache(source, destination, (dsp, dest) => dsp.HasPathTo(dest), havePathByDestinationBySource);
        }

        public long Distance(TVertex source, TVertex destination) {
            return ComputeAndCache(source, destination, (dsp, dest) => dsp.DistanceTo(dest), distancesByDestinationBySource);
        }

        public IEnumerable<TVertex> ShortestPath(TVertex source, TVertex destination) {
            return ComputeAndCache(source, destination, (dsp, dest) => dsp.ShortestPathTo(dest), shortestPathsByDestinationBySource);
        }
    }
}