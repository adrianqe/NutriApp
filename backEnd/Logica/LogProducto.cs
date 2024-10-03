using backEnd.DataAccess;
using backEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace backEnd.Logica
{
    public class LogProducto
    {
        public ResActualizarProducto actualizar(ReqActualizarProducto req)
        {
            ResActualizarProducto res = new ResActualizarProducto();

            try
            {

                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (req.producto == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El producto no puede ser nulo");
                }
                else if (req.producto.Producto_ID <= 0)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID del producto no puede ser menor o igual a cero");
                }
                else if (string.IsNullOrEmpty(req.producto.Nombre))
                {
                    res.exito = false;
                    res.mensaje.Add("El nombre del producto no puede ser nulo o vacío");
                }
            
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Actualizar_Producto(
                        req.producto.Producto_ID,
                        req.producto.Nombre,
                        req.producto.Categoria,
                        req.producto.Marca,
                        req.producto.Informacion_Nutricional,
                        ref exito,
                        ref mensaje
                    );

                    if (exito == true)
                    {
                        res.exito = true;
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add(mensaje);  
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message); 
            }

            return res;
        }

        public ResEliminarProducto eliminar(ReqEliminarProducto req)
        {
            ResEliminarProducto res = new ResEliminarProducto();

            try
            {
                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (req.producto == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El producto no puede ser nulo");
                }
                else if (req.producto.Producto_ID <= 0)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID del producto no puede ser menor o igual a cero");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                 
                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Eliminar_Producto(
                        req.producto.Producto_ID,
                        ref exito,
                        ref mensaje
                    );

                    if (exito == true)
                    {
                        res.exito = true;
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add(mensaje); 
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message);  
            }

            return res;
        }

        public ResConsultarProductosEscaneados consultar(ReqConsultarProductosEscaneados req)
        {
            ResConsultarProductosEscaneados res = new ResConsultarProductosEscaneados();
            res.ProductosEscaneados = new List<Producto>();

            try
            {
               
                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (req.Usuario_ID <= 0)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID del usuario no puede ser menor o igual a cero");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                    
                    ConectionDataContext miLinq = new ConectionDataContext();
                    
                    if (exito == true)
                    {
                        res.exito = true;
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add(mensaje);  
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message);  
            }

            return res;
        }
    }
}
