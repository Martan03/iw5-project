using AutoMapper;
using IW5Forms.Common.Installers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Common.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInstaller<TInstaller>(this IServiceCollection serviceCollection)
            where TInstaller : IInstaller, new()
        {
            var installer = new TInstaller();
            installer.Install(serviceCollection);
        }

        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object?>> selector)
        {
            if (selector is not null)
            {
                map.ForMember(selector, opt => opt.Ignore());
            }
            return map;
        }
    }
}
