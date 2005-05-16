/* canvasitem.c : Glue for accessing fields in a GnomeCanvasItem
 *
 * Author: Duncan Mak (duncan@ximian.com)
 *         Mike Kestner (mkestner@ximian.com)
 *
 * Copyright (C) 2001-2003 Ximian, Inc.
 * Copyright (C) 2004 Novell, Inc.
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of version 2 of the Lesser GNU General 
 * Public License as published by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this program; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

#include <libgnomecanvas/gnome-canvas.h>

void gnomesharp_canvas_item_base_update (GnomeCanvasItem *item, double *affine, ArtSVP *clip_path, int flags);

void 
gnomesharp_canvas_item_base_update (GnomeCanvasItem *item, double *affine, ArtSVP *clip_path, int flags)
{
	GnomeCanvasItemClass *parent = g_type_class_peek_parent (G_OBJECT_GET_CLASS (item));
	if (parent->update)
		(*parent->update) (item, affine, clip_path, flags);
}

void gnomesharp_canvas_item_override_update (GType gtype, gpointer cb);

void 
gnomesharp_canvas_item_override_update (GType gtype, gpointer cb)
{
	GnomeCanvasItemClass *klass = g_type_class_peek (gtype);
	if (!klass)
		klass = g_type_class_ref (gtype);
	((GnomeCanvasItemClass *) klass)->update = cb;
}

gdouble gnomesharp_canvas_item_base_point (GnomeCanvasItem *item, double x, double y, int cx, int cy, GnomeCanvasItem **actual_item);

gdouble 
gnomesharp_canvas_item_base_point (GnomeCanvasItem *item, double x, double y, int cx, int cy, GnomeCanvasItem **actual_item)
{
	GnomeCanvasItemClass *parent = g_type_class_peek_parent (G_OBJECT_GET_CLASS (item));
	return (*parent->point) (item, x, y, cx, cy, actual_item);
}

void gnomesharp_canvas_item_override_point (GType gtype, gpointer cb);

void 
gnomesharp_canvas_item_override_point (GType gtype, gpointer cb)
{
	GnomeCanvasItemClass *klass = g_type_class_peek (gtype);
	if (!klass)
		klass = g_type_class_ref (gtype);
	((GnomeCanvasItemClass *) klass)->point = cb;
}

void gnomesharp_canvas_item_base_realize (GnomeCanvasItem *item);

void 
gnomesharp_canvas_item_base_realize (GnomeCanvasItem *item)
{
	GnomeCanvasItemClass *parent = g_type_class_peek_parent (G_OBJECT_GET_CLASS (item));
	if (parent->realize)
		(*parent->realize) (item);
}

void gnomesharp_canvas_item_override_realize (GType gtype, gpointer cb);

void 
gnomesharp_canvas_item_override_realize (GType gtype, gpointer cb)
{
	GnomeCanvasItemClass *klass = g_type_class_peek (gtype);
	if (!klass)
		klass = g_type_class_ref (gtype);
	((GnomeCanvasItemClass *) klass)->realize = cb;
}

void gnomesharp_canvas_item_base_draw (GnomeCanvasItem *item, GdkDrawable *drawable, int x, int y, int width, int height);

void 
gnomesharp_canvas_item_base_draw (GnomeCanvasItem *item, GdkDrawable *drawable, int x, int y, int width, int height)
{
	GnomeCanvasItemClass *parent = g_type_class_peek_parent (G_OBJECT_GET_CLASS (item));
	if (parent->draw)
		(*parent->draw) (item, drawable, x, y, width, height);
}

void gnomesharp_canvas_item_override_draw (GType gtype, gpointer cb);

void 
gnomesharp_canvas_item_override_draw (GType gtype, gpointer cb)
{
	GnomeCanvasItemClass *klass = g_type_class_peek (gtype);
	if (!klass)
		klass = g_type_class_ref (gtype);
	((GnomeCanvasItemClass *) klass)->draw = cb;
}

void gnomesharp_canvas_item_base_render (GnomeCanvasItem *item, GnomeCanvasBuf *buf);

void 
gnomesharp_canvas_item_base_render (GnomeCanvasItem *item, GnomeCanvasBuf *buf)
{
	GnomeCanvasItemClass *parent = g_type_class_peek_parent (G_OBJECT_GET_CLASS (item));
	if (parent->render)
		(*parent->render) (item, buf);
}

void gnomesharp_canvas_item_override_render (GType gtype, gpointer cb);

void 
gnomesharp_canvas_item_override_render (GType gtype, gpointer cb)
{
	GnomeCanvasItemClass *klass = g_type_class_peek (gtype);
	if (!klass)
		klass = g_type_class_ref (gtype);
	((GnomeCanvasItemClass *) klass)->render = cb;
}

