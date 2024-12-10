$(document).ready(function() {
     // #example tablosunda bir DataTable örneği yoksa başlat
     if (!$.fn.DataTable.isDataTable('#example')) {
        $('#example').DataTable({
            "order": [[0, "desc"]], // 0. sütuna göre azalan sırada sıralama (ID sütunu)
            "columnDefs": [
                { "targets": 0, "visible": false } // İlk sütunu (ID sütunu) gizle
            ],
        "language": {
            "decimal": ",",
            "emptyTable": "Tabloda herhangi bir veri mevcut değil",
            "info": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "infoEmpty": "Kayıt yok",
            "infoFiltered": "(_MAX_ kayıt içerisinden filtrelendi)",
            "infoThousands": ".",
            "lengthMenu": "Sayfada _MENU_ kayıt göster",
            "loadingRecords": "Yükleniyor...",
            "processing": "İşleniyor...",
            "search": "Ara:",
            "zeroRecords": "Eşleşen kayıt bulunamadı",
            "paginate": {
                "first": "İlk",
                "last": "Son",
                "next": "Sonraki",
                "previous": "Önceki"
            },
            "aria": {
                "sortAscending": ": artan sütun sıralamasını aktifleştir",
                "sortDescending": ": azalan sütun sıralamasını aktifleştir"
            }
        },
        "createdRow": function( row, data, dataIndex ) {
            $(row).css('background-color', 'white');
        },
        "initComplete": function(settings, json) {
            $('#example thead th').css('background-color', '#f8f9fa').css('color', '#333');
        }
    });
}
});

//responsive
$(document).ready(function() {
    if (!$.fn.DataTable.isDataTable('#example')) {
      $('#example').DataTable({
        responsive: true
      });
    }
  });

 
  