#encoding : utf-8

from xml.dom.minidom import Document

class xml_node:
	def __init__(self, doc):
		self._doc = doc

	def create(self, name, parent):
		_doc = self._doc.get_doc()

		self._node = _doc.createElement(name)
		if parent == None:
			parent = self._doc.get_root()
		parent.appendChild(self._node)

	def add_child(self, name):
		_node = xml_node(self._doc)
		_node.create(name, self._node)

		return _node

	def add_child_with_value(self, name, value):
		_doc = self._doc.get_doc()

		_node = xml_node(self._doc)
		_node.create(name, self._node)

		_value = _doc.createTextNode(value)
		_node._node.appendChild(_value)

		return _node

	def add_attribute(self, name, value):
		if self._node != None:
			self._node.setAttribute(name, value)

class xml_doc:
	def __init__(self, name):
		self._doc	= Document()
		self._root	= self._doc.createElement(name)

		self._doc.appendChild(self._root)

	def add_node(self, name):
		_node = xml_node(self)
		_node.create(name, None)

		return _node

	def get_doc(self):
		return self._doc

	def get_root(self):
		return self._root

	def save(self, f):
		self._doc.writexml(f, addindent = '\t', newl='\n', encoding='utf-8')

