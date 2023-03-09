<template>
  <div class="container">
    <img src="../assets/bike.png" height="100" />
    <div class="heading">Brands</div>
    <!-- <div class="status">{{ state.status }}</div> -->
    <Dropdown
      v-if="state.brands.length > 0"
      v-model="state.selectedBrand"
      style="text-align: left"
      :options="state.brands"
      optionLabel="name"
      optionValue="id"
      placeholder="Select Brand"
      @change="loadProduct"
    />

    <div v-if="state.products.length > 0">
      <DataTable
        :value="state.products"
        :scrollable="true"
        scrollHeight="55vh"
        selectionMode="single"
        class="p-datatable-striped"
        @row-select="onRowSelect"
      >
        <Column style="margin-right: -35vw">
          <template #body="slotProps">
            <img
              :src="`/img/${slotProps.data.graphicName}`"
              :alt="slotProps.data.productName"
              class="product-image"
            />
          </template>
        </Column>
        <Column field="productName" header="Bike Name" headerStyle="font-family: 'Aclonica', sans-serif;"></Column>
      </DataTable>
      <Dialog v-model:visible="state.itemSelected" class="dialog-border">
        <div style="text-align: center">
          <img
            :src="`/img/${state.selectedItem.graphicName}`"
            class="selected-product-img"
          />
          <div
            style="
              margin: 2vh;
              font-size: larger;
              font-weight: bold;
              color: rgb(34, 72, 75);
            "
          >
            {{ state.selectedItem.productName }} -
            {{ formatCurrency(state.selectedItem.msrp) }}
          </div>
        </div>

        <!-- Product Description -->
        <div style="margin-top: 3vh; text-align: center; ">
          {{ state.selectedItem.description }}
        </div>

        <!-- InputNumber  -->
        <div style="margin-top: 2vh; text-align: center;margin-left: -5vw">
          <span style="margin-right: 2vw">Qty:</span>
          <span>
            <InputNumber
              id="qty"
              :min="0"
              v-model="state.qty"
              :step="1"
              incrementButtonClass="incDec"
              decrementButtonClass="incDec"
              showButtons
              buttonLayout="horizontal"
              decrementButtonIcon="pi pi-minus"
              incrementButtonIcon="pi pi-plus"
            />
          </span>
        </div>

        <!-- Add To Cart and View Cart Button -->
        <div style="text-align: center; margin-top: 2vh">
          <Button
            label="Add To Cart"
            @click="addToCart"
            class="p-button-outlined margin-button1"
            style="color: rgb(34, 72, 75)"
          />
          &nbsp;
          <Button
            label="View Cart"
            class="p-button-outlined margin-button1"
            v-if="state.cart.length > 0"
            @click="viewCart"
            style="color: rgb(34, 72, 75)"
          />
        </div>

        <!-- Status -->
        <div
          style="text-align: center"
          v-if="state.dialogStatus !== ''"
          class="dialog-status"
        >
          {{ state.dialogStatus }}
        </div>
      </Dialog>
    </div>
  </div>
</template>
<script>
import { reactive, onMounted } from "vue";
import { fetcher } from "../util/apiutil";
import { useRouter } from "vue-router";
export default {
  setup() {
    const router = useRouter();
    onMounted(() => {
      loadBrands();
    });
    let state = reactive({
      status: "",
      brands: [],
      products: [],
      selectedBrand: {},
      selectedItem: {},
      dialogStatus: "",
      itemSelected: false,
      qty: 0,
      cart: [],
    });
    const loadBrands = async () => {
      try {
        state.status = "finding brands ...";
        const payload = await fetcher(`Brand`);
        if (payload.error === undefined) {
          state.brands = payload;
          state.status = `loaded ${state.brands.length} brands`;
        } else {
          state.status = payload.error;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const loadProduct = async () => {
      try {
        state.status = `finding product for brand ${state.selectedBrand}...`;
        let payload = await fetcher(`Product/${state.selectedBrand}`);
        state.products = payload;
        state.status = `loaded ${state.products.length} products`;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
      if (sessionStorage.getItem("cart")) {
        state.cart = JSON.parse(sessionStorage.getItem("cart"));
      }
    };

    const onRowSelect = (event) => {
      try {
        state.selectedItem = event.data;
        state.dialogStatus = "";
        state.itemSelected = true;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    const formatCurrency = (value) => {
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    };
    const addToCart = () => {
      const index = state.cart.findIndex(
        // is item already on the cart
        (item) => item.id === state.selectedItem.id
      );
      if (state.qty !== 0) {
        index === -1 // add
          ? state.cart.push({
              id: state.selectedItem.id,
              qty: state.qty,
              item: state.selectedItem,
            })
          : (state.cart[index] = {
              // replace
              id: state.selectedItem.id,
              qty: state.qty,
              item: state.selectedItem,
            });
        state.dialogStatus = `${state.qty} item(s) added`;
      } else {
        index === -1 ? null : state.cart.splice(index, 1); // remove
        state.dialogStatus = `item(s) removed`;
      }
      sessionStorage.setItem("cart", JSON.stringify(state.cart));
      state.qty = 0;
    };

    const viewCart = () => {router.push("cart");};

    return {
      state,
      loadProduct,
      onRowSelect,
      formatCurrency,
      addToCart,
      viewCart
    };
  },
};
</script>
