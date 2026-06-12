# language: es
  # Author: Cristian

  Característica: Consultar un libro en el catálogo
    Como administrador del sistema
    Quiero poder consultar un libro por su nombre
    Para encontrar información de libros en el catálogo

  Antecedentes:
    Dado el administrador inicie sesion con las credenciales correctas
      | usuario              | contraseña |
      | admin@biblioteca.com | Admin1234  |
    Y se encuentre en la pagina de gestion gestion de catalogo

  @ConsultarLibro
  Escenario: Buscar libro por nombre
    Dado que existe un libro
    Cuando e ingresa el nombre del libro
    Entonces deberia ver unicamente el libro con su nombre