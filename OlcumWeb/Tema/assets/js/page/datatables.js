"use strict";

$("[data-checkboxes]").each(function () {
  var me = $(this),
    group = me.data('checkboxes'),
    role = me.data('checkbox-role');

  me.change(function () {
    var all = $('[data-checkboxes="' + group + '"]:not([data-checkbox-role="dad"])'),
      checked = $('[data-checkboxes="' + group + '"]:not([data-checkbox-role="dad"]):checked'),
      dad = $('[data-checkboxes="' + group + '"][data-checkbox-role="dad"]'),
      total = all.length,
      checked_length = checked.length;

    if (role == 'dad') {
      if (me.is(':checked')) {
        all.prop('checked', true);
      } else {
        all.prop('checked', false);
      }
    } else {
      if (checked_length >= total) {
        dad.prop('checked', true);
      } else {
        dad.prop('checked', false);
      }
    }
  });
});

$("#table-1").dataTable({
  "columnDefs": [
    { "sortable": false, "targets": [2, 3] }
  ]
});
//$("#table-2").dataTable({
//  "columnDefs": [
//    { "sortable": false, "targets": [0, 2, 3] }
//  ],
//  order: [[1, "asc"]] //column indexes is zero based

//});
//$('#save-stage').DataTable({
//  "scrollX": true,
//  stateSave: true
//});
$('#tableExport').DataTable({
  dom: 'Bfrtip',
  buttons: [
    'copy', 'csv', 'excel', 'pdf', 'print'
  ]
});

$('#save-stage').DataTable({
    "dom": '<"top"<"row mr-0 ml-0 mt-1 mb-1"<"col-6 m-auto"f><"col-6 m-auto"l>>>rt<"row mt-2 mr-0 mb-0 ml-0"<"col - 6 m- auto"i><"col - 6 m - auto"p>><"clear">',
    "columnDefs": [{
        "targets": 0,
        "orderable": false
    }],
    "pageLength": 5,
    "lengthMenu": [5, 10, 20, 50, 100],
    "language": {
        "searchPlaceholder": "Arama",
        "search": "",
        "info": "TOTAL Kayýt içerisinde START - END gösteriliyor",
        "loadingRecords": "Yükleniyor...",
        "zeroRecords": "Kayýt Bulunamadý",
        "lengthMenu": "MENU",
        "infoEmpty": "",
        "infoFiltered": "",
        "paginate": {
            "first": "Ýlk Sayfa",
            "last": "Son Sayfa",
            "next": "Sonraki Sayfa",
            "previous": "Önceki Sayfa",
        },
    }
}).columns(-1).order('desc').draw();
