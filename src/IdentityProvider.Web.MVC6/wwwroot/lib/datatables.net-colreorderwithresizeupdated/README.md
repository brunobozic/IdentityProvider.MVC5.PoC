# 

<div align="center">
  <h1>ColReorderResizeNPM</h1>

  This is a simple fork of jeffreydwalter's ColReorderWithResize package that adds NPM support
</div>
  Other Columns Will not be resized when scrollX is true.

<hr />

[![version](https://img.shields.io/npm/v/datatables.net-colreorderwithresizeupdated.svg?style=flat-square)](https://www.npmjs.com/package/datatables.net-colreorderwithresizeupdated)
[![downloads](https://img.shields.io/npm/dm/datatables.net-colreorderwithresizeupdated.svg?style=flat-square)](http://npm-stat.com/charts.html?package=datatables.net-colreorderwithresizeupdated&from=2020-06-23)
[![MIT License](https://img.shields.io/npm/l/ColReorderWithResizeNPM.svg?style=flat-square)](https://github.com/RishabhSachan/ColReorderWithResizeNPM/blob/master/LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)

ColReorderResize adds the ability for the end user to click and drag column headers to reorder and resize a table as they see fit, to DataTables. See the [documentation](http://legacy.datatables.net/extras/thirdparty/ColReorderWithResize/) for full details.

This repository is based on a no longer supported version of the ColReorderWithResize plugin. I basically updated it to work with the latest version of DataTables and fixed some long-standing issues with the plugin.

There is an example of using this plugin here: https://www.gyrocode.com/articles/jquery-datatables-column-reordering-and-resizing

Plug-in can be initialized multiple ways:

Using dom option and adding character R.
```
var table = $('#example').DataTable({
    'dom': 'Rlfrtip'
});
```
Using new $.fn.dataTable.ColReorder().
```
var table = $('#example').DataTable();
new $.fn.dataTable.ColReorder(table);
```

This plugin fires the following events:
```
column-reorder.dt
column-reorder.dt.mouseup
column-resize.dt.mouseup
mousemove.ColReorder
touchmove.ColReorder
mouseup.ColReorder
touchend.ColReorder
```

Here is an example of capturing a resize event:
```
// Do something when a resize occurs.
$('#datatable').on('column-resize.dt.mouseup', function(event, oSettings) {
    // Do something here.
});
```
Here is an example of capturing some reorder event:
```
$('#datatable').on('column-reorder.dt.mouseup', function(event, oSettings) {
    // Do something here.
});
    
// or 
    
$('.dataTables_wrapper')
    .on('mousedown.ColReorder touchstart.ColReorder', function(event) {
        // User has clicked on a column and is currently holding the mouse button down.
    })
    .on('mouseup.ColReorder touchend.ColReorder', function(event) {
        // Users has released the mouse button.
    });
```
