
$(document).ready(function(){
    $('#tbCategorias').DataTable({
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.5/i18n/es-ES.json" // Tradicción al español
        },
        pageLength: 10, // Cantidad de registros por página
        ordering: true, //Habilitar ordenamiento
        searching: true //Habilitar la busqueda
    })
});
