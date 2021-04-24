using csDelaunay;
using Damrem.Collections;
using System;
using System.Collections.Generic;

namespace Damrem.Delaunay {

    [Obsolete]
    public static class VoronoiExt {
        public static List<Site> GettSites(this Voronoi voronoi) {
            if (voronoi == null)
                return new List<Site>();

            return voronoi.SiteCoords()
                .FindAll(coord => voronoi.SitesIndexedByLocation != null)
                .Map(coord => voronoi.SitesIndexedByLocation[coord]);
        }

        public static EditableVoronoi Clone(this EditableVoronoi original) {
            return new EditableVoronoi(original.SiteCoords(), original.PlotBounds);
        }
    }
}