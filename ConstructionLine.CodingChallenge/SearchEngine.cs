﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;        

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
        }

        public SearchResults Search(SearchOptions options)
        {
            // TODO: search logic goes here.
            var results = _shirts.Where(s => options.Colors.Contains(s.Color) 
                                || options.Sizes.Contains(s.Size)).ToList();
                        
           
            return new SearchResults
            {
                ColorCounts = GetColorCounts(options).ToList(),
                Shirts = results,
                SizeCounts = GetSizeCounts(options).ToList()
            };
        }

        private ConcurrentBag<SizeCount> GetSizeCounts(SearchOptions options)
        {
          
            var sizeCounts = new ConcurrentBag<SizeCount>();
            Parallel.ForEach(Size.All, size =>
            {
                sizeCounts.Add(new SizeCount()
                {
                    Size = size,
                    Count = _shirts
                                    .Count(s => s.Size.Id == size.Id
                                    && (!options.Colors.Any() ||
                                         options.Colors.Select(c => c.Id).Contains(s.Color.Id)))
                });
            });


            return sizeCounts;
        }

        private ConcurrentBag<ColorCount> GetColorCounts(SearchOptions options)
        {
            
            var colorCounts = new ConcurrentBag<ColorCount>();
            Parallel.ForEach(Color.All, color =>
            {
                colorCounts.Add(new ColorCount()
                {
                    Color = color,
                    Count = _shirts
                                .Count(c => c.Color.Id == color.Id
                                && (!options.Sizes.Any() ||
                                     options.Sizes.Select(s => s.Id).Contains(c.Size.Id)))
                });
            });

            return colorCounts;

        }
    }
}
