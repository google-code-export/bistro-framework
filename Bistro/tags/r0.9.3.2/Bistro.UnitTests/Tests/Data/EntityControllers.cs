using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Validation;
using Bistro.Controllers.Descriptor;
using Bistro.Extensions.Validation;
using Bistro.Extensions.Validation.Common;
using System.Text.RegularExpressions;
using Bistro.Controllers.Descriptor.Data;
using Bistro.Controllers;
using Bistro.Entity;

namespace Bistro.UnitTests.Tests.Data
{
    public sealed class SimpleEntityValidator : Validator<SimpleEntity>
    {
        public SimpleEntityValidator()
        {
            As("Something")
            .Define(
                Value(c => c.foo).IsRequired("Foo is required")
                );
        }
    }

    public interface ISimpleEntity : IValidatable
    {
        string foo { get; set;}
        string bar { get; set; }
        string baz { get; set; }
        string extra { get; set; }
    }

    [ValidateWith(typeof(SimpleEntityValidator))]
    public class SimpleEntity: ISimpleEntity
    {

		public int? nullableInt { get; set; }
		public bool? nullableBool { get; set; }
		public string foo { get; set; }
        public string bar { get; set; }
        public string baz { get; set; }
        public string extra { get; set; }

        public List<IValidationResult> Messages { get; set; }
        public bool IsValid { get; set; }
    }

    class TestMapper : EntityMapper<EntityController, SimpleEntity>
    {
        public TestMapper()
        {
            Infer()
                .Except(x => x.extra)
                .Map(x => x.thirdField).To(y => y.baz);
        }
    }

    sealed class EntityControllerValidator : Validator<EntityController>
    {
        public EntityControllerValidator() { ByMapping(); }
    }

    [Bind("/entityTest?{Foo}&{bar}&{thirdField}&{extra}&{unwrap}&{nullableInt}&{nullableBool}&{MyDate}")]
    [ValidateWith(typeof(EntityControllerValidator))]
    [MapsWith(typeof(TestMapper))]
    public class EntityController : AbstractController, IMappable, IValidatable
    {
		[Request]
		public DateTime MyDate;

        [Request]
        public string
            Foo,
            bar,
            thirdField;

		[FormField]
		public int? nullableInt;

		[FormField]
		public bool? nullableBool;

        public string extra;

        public bool unwrap;

        [FormField, Request]
        public SimpleEntity entity;

        [Request]
        public List<IValidationResult> Messages { get; set; }

        [Request]
        public bool IsValid { get; set; }

        public override void DoProcessRequest(IExecutionContext context)
        {
            if (unwrap)
                Mapper.Unmap(this, entity);
            else
            {
                if (!IsValid)
                    return;

                entity = new SimpleEntity();
                Mapper.Map(this, entity);

                context.Response.Return(entity);
            }
        }

        public EntityMapperBase Mapper { get; set; }
    }

    class StrictTestMapper : EntityMapper<StrictEntityController, SimpleEntity>
    {
        public StrictTestMapper()
        {
            InferStrict()
                .Map(x => x.thirdField).To(y => y.baz);
        }
    }

    [Bind("/strictEntityTest?{Foo}&{bar}&{thirdField}&{unwrap}")]
    [MapsWith(typeof(StrictTestMapper))]
    public class StrictEntityController : AbstractController, IMappable, IValidatable
    {
        [Request]
        public string
            Foo,
            bar,
            thirdField;

        public bool unwrap;

        [FormField, Request]
        public SimpleEntity entity;

        [Request]
        public List<IValidationResult> Messages { get; set; }

        [Request]
        public bool IsValid { get; set; }

        public override void DoProcessRequest(IExecutionContext context)
        {
            if (unwrap)
                Mapper.Unmap(this, entity);
            else
            {
                if (!IsValid)
                    return;

                entity = new SimpleEntity();
                Mapper.Map(this, entity);

                context.Response.Return(entity);
            }
        }

        public EntityMapperBase Mapper { get; set; }
    }
    [Bind("/attributeInferredEntityTest?{Foo}&{bar}&{unwrap}")]
    [InferMappingFor(typeof(SimpleEntity))]
    [InferValidationFrom(typeof(SimpleEntity))]
    public class InferredEntityController : AbstractController, IMappable, IValidatable
    {
        [Request]
        public string
            Foo,
            bar;

        public bool unwrap;

        [FormField, Request]
        public SimpleEntity entity;

        [Request]
        public List<IValidationResult> Messages { get; set; }

        [Request]
        public bool IsValid { get; set; }

        public override void DoProcessRequest(IExecutionContext context)
        {
            if (unwrap)
                Mapper.Unmap(this, entity);
            else
            {
                if (!IsValid)
                    return;

                entity = new SimpleEntity();
                Mapper.Map(this, entity);

                context.Response.Return(entity);
            }
        }

        public EntityMapperBase Mapper { get; set; }
    }


    [Bind("/attributeInterfaceEntityTest?{Foo}&{bar}&{unwrap}")]
    [InferMappingFor(typeof(ISimpleEntity))]
    public class InferredInterfaceController : AbstractController, IMappable, IValidatable
    {
        [Request]
        public string
            Foo,
            bar;

        public bool unwrap;

        [FormField, Request]
        public ISimpleEntity entity;

        [Request]
        public List<IValidationResult> Messages { get; set; }

        [Request]
        public bool IsValid { get; set; }

        public override void DoProcessRequest(IExecutionContext context)
        {
            if (unwrap)
                Mapper.Unmap(this, entity);
            else
            {
                if (!IsValid)
                    return;

                entity = new SimpleEntity();
                Mapper.Map(this, entity);

                context.Response.Return(entity);
            }
        }

        public EntityMapperBase Mapper { get; set; }
    }
}
