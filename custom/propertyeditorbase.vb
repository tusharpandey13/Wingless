﻿Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports System.ComponentModel


''' <summary> 
''' This is a UITypeEditor base class usefull for simple editing of control 
''' properties in a DropDown or a ModalForm window at design mode (in 
''' Visual Studio IDE). To use this, inherits a class from it and add this
''' attribute to your control property(ies): 
''' <Editor(GetType(MyPropertyEditor), GetType(System.Drawing.Design.UITypeEditor))></Editor>
''' </summary>

Public MustInherit Class PropertyEditorBase
    Inherits System.Drawing.Design.UITypeEditor

    ''' <summary>
    ''' The driven class should provide its edit Control to be shown in the 
    ''' DropDown or DialogForm window by means of this function. 
    ''' If specified control be a Form, it is shown in a Modal Form, otherwise 
    ''' in a DropDown window. This edit control should return its final value 
    ''' at GetEditedValue() method. 
    ''' </summary>

    Protected MustOverride Function GetEditControl(ByVal PropertyName As _
      String, ByVal CurrentValue As Object) As Control

    ''' <summary>The driven class should return the New Edited Value at this 
    ''' function.</summary>
    ''' <param name="EditControl">
    ''' The control shown in DropDown window and used for editing. 
    ''' This is the control you pass in GetEditControl() function.
    ''' </param>
    ''' <param name="OldValue">The original value of the property before 
    ''' editing</param>

    Protected MustOverride Function GetEditedValue(ByVal EditControl As _
      Control, ByVal PropertyName As String,
               ByVal OldValue As Object) As Object

    Protected IEditorService As IWindowsFormsEditorService
    Private WithEvents m_EditControl As Control
    Private m_EscapePressed As Boolean

    ''' <summary>
    ''' Sets the edit style mode based on the type of EditControl: DropDown or
    ''' Modal(Dialog). 
    ''' Note that the driven class can also override this function and 
    ''' explicitly set its value.
    ''' </summary>

    Public Overrides Function GetEditStyle(ByVal context As _
                               ITypeDescriptorContext) As UITypeEditorEditStyle
        Try
            Dim c As Control
            c = GetEditControl(context.PropertyDescriptor.Name,
                context.PropertyDescriptor.GetValue(context.Instance))
            If TypeOf c Is Form Then
                Return UITypeEditorEditStyle.Modal 'Using a Modal Form
            End If
        Catch
        End Try
        'Using a DropDown Window (This is the default style)
        Return UITypeEditorEditStyle.DropDown
    End Function

    'Displays the Custom UI (a DropDown Control or a Modal Form) for value 
    'selection.
    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

        Try
            If context IsNot Nothing AndAlso provider IsNot Nothing Then

                'Uses the IWindowsFormsEditorService to display a drop-down
                'UI in the Properties window:
                IEditorService = DirectCast(
                provider.GetService(GetType(IWindowsFormsEditorService)),
                                    IWindowsFormsEditorService)

                If IEditorService IsNot Nothing Then

                    Dim PropName As String = context.PropertyDescriptor.Name

                    'get Edit Control from driven class
                    m_EditControl = Me.GetEditControl(PropName, value)

                    If m_EditControl IsNot Nothing Then

                        m_EscapePressed = False

                        'Show given EditControl
                        If TypeOf m_EditControl Is Form Then
                            IEditorService.ShowDialog(CType(m_EditControl, Form))
                        Else
                            IEditorService.DropDownControl(m_EditControl)
                        End If

                        If m_EscapePressed Then    'return the Old Value 
                            '(because user press Escape)
                            Return value
                        Else 'get new (edited) value from driven class and 
                            'return it
                            Return GetEditedValue(m_EditControl, PropName, value)
                        End If

                    End If 'm_EditControl

                End If 'IEditorService

            End If 'context And provider

        Catch ex As Exception
            'we may show a MessageBox here...
        End Try
        Return MyBase.EditValue(context, provider, value)

    End Function

    ''' <summary>
    ''' Provides the interface for this UITypeEditor to display Windows Forms 
    ''' or to display a control in a DropDown area from the property grid 
    ''' control in design mode.
    ''' </summary>

    Public Function GetIWindowsFormsEditorService() As _
                                                   IWindowsFormsEditorService
        Return IEditorService
    End Function

    ''' <summary>Close DropDown window to finish editing</summary>

    Public Sub CloseDropDownWindow()
        If IEditorService IsNot Nothing Then IEditorService.CloseDropDown()
    End Sub

    Private Sub m_EditControl_PreviewKeyDown(ByVal sender As Object,
                                 ByVal e As PreviewKeyDownEventArgs) _
                                      Handles m_EditControl.PreviewKeyDown
        If e.KeyCode = Keys.Escape Then m_EscapePressed = True
    End Sub

End Class