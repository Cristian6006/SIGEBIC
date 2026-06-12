# language: es
  # Author: Cristian

  Característica: Actualizar datos de un libro en el catálogo
    Como administrador del sistema
    Quiero poder actualizar la información de un libro existente
    Para mantener el catálogo actualizado

  Antecedentes:
    Dado el administrador inicie sesion con las credenciales correctas
      | usuario              | contraseña |
      | admin@biblioteca.com | Admin1234  |
    Y se encuentre en la pagina de gestion gestion de catalogo

  @ActualizarLibro
  Escenario: Actualizar datos de un libro
    Cuando el administrador modifica la informacion de un libro existente en el catalogo.
    Entonces deberia los datos del libro actualizodos