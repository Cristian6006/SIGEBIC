# language: es
  # Author: Cristian

  Característica: Eliminar un libro del catálogo
    Como administrador del sistema
    Quiero poder eliminar un libro del sistema
    Para mantener el catálogo actualizado

  Antecedentes:
    Dado el administrador inicie sesion con las credenciales correctas
      | usuario              | contraseña |
      | admin@biblioteca.com | Admin1234  |
    Y se encuentre en la pagina de gestion gestion de catalogo

  @BorrarLibro
  Escenario: Eliminar un libro
    Dado que existe un libro
    Cuando elimina a el libro del sistema
    Entonces deberia ver que el usuario no exista en la lista