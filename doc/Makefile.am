if ENABLE_MONODOC
TARGETS = gtk-sharp-docs.zip gtk-sharp-docs.tree gtk-sharp-docs.source
DOCDIR = `$(MONODOC) --get-sourcesdir`
else
TARGETS = 
DOCDIR = $(datadir)
endif

noinst_DATA = $(TARGETS)

ASSEMBLER = $(MONODOC) --assemble
UPDATER = $(MONODOC) --update

DIRS=glib pango atk gdk gtk glade art gnome gda gnomedb gconf/GConf gconf/GConf.PropertyEditors rsvg gtkhtml

gtk-sharp-docs.zip gtk-sharp-docs.tree: $(srcdir)/en/*/*.xml $(srcdir)/en/*.xml
	$(ASSEMBLER) --ecma $(srcdir)/en -o gtk-sharp-docs

update:
	for i in $(DIRS); do                    		\
		$(UPDATER) ../$$i/*.dll -o ./en -f || exit 1;	\
        done

sources =			\
	Tool.cs

build_sources = $(addprefix $(srcdir)/, $(sources))

CLEANFILES = gtk-sharp-docs.zip gtk-sharp-docs.tree

EXTRA_DIST =			\
	gtk-sharp-docs.source

dist-hook:
	mkdir -p $(distdir)/en
	cp $(srcdir)/en/*.xml $(distdir)/en/
	mkdir -p $(distdir)/en/GLib
	cp $(srcdir)/en/GLib/*.xml $(distdir)/en/GLib/
	mkdir -p $(distdir)/en/Pango
	cp $(srcdir)/en/Pango/*.xml $(distdir)/en/Pango/
	mkdir -p $(distdir)/en/Atk
	cp $(srcdir)/en/Atk/*.xml $(distdir)/en/Atk/
	mkdir -p $(distdir)/en/Gdk
	cp $(srcdir)/en/Gdk/*.xml $(distdir)/en/Gdk/
	mkdir -p $(distdir)/en/Gtk
	cp $(srcdir)/en/Gtk/*.xml $(distdir)/en/Gtk/
	mkdir -p $(distdir)/en/Glade
	cp $(srcdir)/en/Glade/*.xml $(distdir)/en/Glade/
	mkdir -p $(distdir)/en/Art
	cp $(srcdir)/en/Art/*.xml $(distdir)/en/Art/
	mkdir -p $(distdir)/en/Gnome
	cp $(srcdir)/en/Gnome/*.xml $(distdir)/en/Gnome/
	mkdir -p $(distdir)/en/Gda
	cp $(srcdir)/en/Gda/*.xml $(distdir)/en/Gda/
	mkdir -p $(distdir)/en/GnomeDb
	cp $(srcdir)/en/GnomeDb/*.xml $(distdir)/en/GnomeDb/
	mkdir -p $(distdir)/en/Rsvg
	cp $(srcdir)/en/Rsvg/*.xml $(distdir)/en/Rsvg/
	mkdir -p $(distdir)/en/Vte
	cp $(srcdir)/en/Vte/*.xml $(distdir)/en/Vte/
	mkdir -p $(distdir)/en/GConf
	cp $(srcdir)/en/GConf/*.xml $(distdir)/en/GConf/
	mkdir -p $(distdir)/en/GConf.PropertyEditors
	cp $(srcdir)/en/GConf.PropertyEditors/*.xml $(distdir)/en/GConf.PropertyEditors/
	mkdir -p $(distdir)/en/GLibSharp
	cp $(srcdir)/en/GLibSharp/*.xml $(distdir)/en/GLibSharp/
	mkdir -p $(distdir)/en/GtkSharp
	cp $(srcdir)/en/GtkSharp/*.xml $(distdir)/en/GtkSharp/