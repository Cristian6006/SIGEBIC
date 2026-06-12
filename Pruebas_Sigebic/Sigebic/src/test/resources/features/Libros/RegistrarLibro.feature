# language: es
  # Author: Cristian

  Característica: Registrar un nuevo libro en el catálogo
    Como administrador del sistema
    Quiero poder registrar un nuevo libro en el catálogo
    Para mantener actualizado el inventario de libros

  Antecedentes:
    Dado el administrador inicie sesion con las credenciales correctas
      | usuario              | contraseña |
      | admin@biblioteca.com | Admin1234  |
    Y se encuentre en la pagina de gestion gestion de catalogo

  @AgregarUsuario
  Escenario: Crear un nuevo libro
    Cuando el administrador crea un nuevo libro
    Entonces deberia ver el nuevo usuario en la lista