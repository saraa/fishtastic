assemblydir = $(prefix)/lib/gtk-sharp-2.0
assembly_DATA = gapi_codegen.exe
bin_SCRIPTS = gapi2-codegen
CLEANFILES = gapi_codegen.exe
DISTCLEANFILES = gapi2-codegen

references =

sources = 				\
	AliasGen.cs			\
	BoxedGen.cs			\
	ByRefGen.cs			\
	CallbackGen.cs			\
	ChildProperty.cs		\
	ClassBase.cs			\
	ClassField.cs			\
	ClassGen.cs			\
	CodeGenerator.cs		\
	ConstFilenameGen.cs		\
	ConstStringGen.cs		\
	Ctor.cs				\
	DefaultSignalHandler.cs		\
	EnumGen.cs			\
	FieldBase.cs			\
	GenBase.cs			\
	GenerationInfo.cs		\
	GObjectVM.cs			\
	HandleBase.cs			\
	IAccessor.cs			\
	IGeneratable.cs			\
	IManualMarshaler.cs		\
	InterfaceGen.cs			\
	InterfaceVM.cs			\
	LPGen.cs			\
	LPUGen.cs			\
	ManagedCallString.cs		\
	ManualGen.cs			\
	MarshalGen.cs			\
	MethodBase.cs			\
	MethodBody.cs			\
	Method.cs			\
	ObjectField.cs			\
	ObjectBase.cs			\
	ObjectGen.cs			\
	OpaqueGen.cs			\
	Parameters.cs			\
	Parser.cs			\
	Property.cs			\
	PropertyBase.cs			\
	ReturnValue.cs			\
	Signal.cs			\
	Signature.cs			\
	SimpleBase.cs			\
	SimpleGen.cs			\
	Statistics.cs			\
	StructBase.cs			\
	StructField.cs			\
	StructGen.cs			\
	SymbolTable.cs			\
	VirtualMethod.cs		\
	VMSignature.cs

build_sources = $(addprefix $(srcdir)/, $(sources))
dist_sources = $(sources)

EXTRA_DIST =					\
	$(dist_sources)

gapi_codegen.exe: $(build_sources)
	$(CSC) /out:gapi_codegen.exe $(OFF_T_FLAGS) $(references) $(build_sources)

